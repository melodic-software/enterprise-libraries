using Enterprise.Options.Core.Abstract;
using Enterprise.Serialization.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Enterprise.Library.Core.Disposables;

namespace Enterprise.Options;

/// <summary>
/// This dynamically monitors configuration options of a specific type.
/// This should be used for any options that need to be retrieved after the DI container has been created.
/// Use <see cref="Core.Singleton.OptionsInstanceService"/> when the application has not yet been built.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class DynamicOptionsMonitor<TOptions> : 
    IOptionsMonitor<TOptions>, 
    IOptionsUpdater<TOptions>, IDisposable
    where TOptions : class, new()
{
    private bool _disposed = false;
    private readonly TimeSpan _debouncePeriod;
    private readonly ISerializeJson _jsonSerializer;
    private string _currentHash;

    private TOptions _currentValue;
    private readonly IConfigurationSection? _configurationSection;
    private readonly ILogger<DynamicOptionsMonitor<TOptions>> _logger;
    private readonly ConcurrentDictionary<Guid, Action<TOptions, string?>> _onChangeHandlers = new();
    private readonly object _updateLock = new();
    private Timer? _debounceTimer;

    public TOptions CurrentValue => _currentValue;
    public TOptions Value => CurrentValue;
    TOptions IOptionsSnapshot<TOptions>.Get(string? name) => CurrentValue;
    TOptions IOptionsMonitor<TOptions>.Get(string? name) => CurrentValue;

    public DynamicOptionsMonitor(TOptions? currentValue,
        IConfigurationSection? configurationSection,
        ILogger<DynamicOptionsMonitor<TOptions>> logger,
        TimeSpan debouncePeriod,
        ISerializeJson jsonSerializer)
    {
        _configurationSection = configurationSection;
        _logger = logger;
        _debouncePeriod = debouncePeriod;
        _jsonSerializer = jsonSerializer;
        _currentValue = currentValue ?? new TOptions();

        Type optionsType = typeof(TOptions);

        using (_logger.BeginScope("OptionsType: {OptionsType}", optionsType.Name))
        {
            try
            {
                if (_configurationSection == null)
                {
                    _currentHash = ComputeHashForOptions(_currentValue);
                    return;
                }

                BindConfig();

                _currentHash = ComputeHashForOptions(_currentValue);

                ChangeToken.OnChange(() => _configurationSection.GetReloadToken(), ReloadConfiguration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "An error occurred constructing the dynamic options monitor for {OptionsType}.",
                    optionsType.Name
                );

                throw;
            }
        }
    }

    public IDisposable OnChange(Action<TOptions, string?> listener)
    {
        Guid key = Guid.NewGuid();

        lock (_updateLock)
        {
            _logger.LogInformation("Adding configuration change listener.");
            _onChangeHandlers.TryAdd(key, listener);
        }

        // Return an IDisposable that removes the handler when disposed.
        return new DisposableAction(() => _onChangeHandlers.TryRemove(key, out Action<TOptions, string>? _));
    }

    public void UpdateOptions(Action<TOptions> applyChanges)
    {
        lock (_updateLock)
        {
            Type optionsType = typeof(TOptions);
            using (_logger.BeginScope("OptionsType: {OptionsType}", optionsType.Name))
            {
                _logger.LogInformation("Applying configuration changes.");
                applyChanges(_currentValue);
                NotifyChangeSubscribers();
                _logger.LogInformation("Configuration changes applied.");
            }
        }
    }

    private void BindConfig()
    {
        if (_configurationSection == null)
            return;

        // This will overwrite any property values with those found in the config section.

        // WARNING: Collection based properties can result in duplicate values being added.
        // This was discovered with a List<string>. The fix was to change the type to a HashSet<string>.
        // This method was later added to accomodate for clearing collections.
        // TODO: We need to test this. Commenting out for now...
        //CollectionClearer.ClearCollections(_currentValue);

        _configurationSection.Bind(_currentValue);

        // We can use this as an alternative to deduplicate the collection, but it uses reflection.
        //EnumerableDeDuplicationService.DeduplicateIEnumerables(_currentValue);
    }

    private void ReloadConfiguration()
    {
        lock (_updateLock)
        {
            // Dispose of any existing timer to reset the debounce period.
            // This essentially cancels any pending reload.
            _debounceTimer?.Dispose();

            // Create a new timer that waits for the debounce period before invoking the reload.
            _debounceTimer = new Timer(
                _ => DoReloadConfiguration(),
                null,
                _debouncePeriod,
                Timeout.InfiniteTimeSpan
            );
        }
    }

    private void DoReloadConfiguration()
    {
        lock (_updateLock)
        {
            _logger.LogInformation(
                "The application settings JSON has been modified." +
                " Attempting to reload configuration."
            );

            TOptions newInstance = new TOptions();
            _configurationSection?.Bind(newInstance);

            // TODO: Consider performance optimizations here.
            // Computing and comparing hashes may be faster, particularly for large config objects.
            // Another option is to add derived types that specify specific properties and manually do the comparison.
            // We could also treat these as value objects and do equality checks without requiring serialization.
            // This might require some type constraints (via marker interfaces, etc.).

            bool configChanged = ConfigChanged(newInstance, out string newHash);

            if (configChanged)
            {
                _logger.LogInformation("Configuration section '{SectionName}' has changed. Reloading.", _configurationSection?.Path);
                _currentValue = newInstance;
                _currentHash = newHash;
                NotifyChangeSubscribers();
            }
            else
            {
                _logger.LogInformation("No relevant changes detected in the configuration section '{SectionName}'.", _configurationSection?.Path);
            }

            _logger.LogInformation("Configuration reloaded.");
        }
    }

    private void NotifyChangeSubscribers()
    {
        _logger.LogInformation("Notifying subscribers that a config change has occurred.");

        foreach (Action<TOptions, string?> handler in _onChangeHandlers.Values)
        {
            try
            {
                handler(_currentValue, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred during subscriber notification.");
            }
        }

        _logger.LogInformation("Completed notifying subscribers about config changes.");
    }

    private bool ConfigChanged(TOptions newInstance)
    {
        string oldValues = _jsonSerializer.Serialize(_currentValue);
        string newValues = _jsonSerializer.Serialize(newInstance);

        // This comparison is less performant than the hash comparison.
        bool configChanged = !string.Equals(oldValues, newValues, StringComparison.Ordinal);

        return configChanged;
    }

    private bool ConfigChanged(TOptions newInstance, out string newHash)
    {
        newHash = ComputeHashForOptions(newInstance);
        bool configChanged = !string.Equals(_currentHash, newHash, StringComparison.Ordinal);
        return configChanged;
    }

    private string ComputeHashForOptions(object options)
    {
        Dictionary<string, object?> properties = options.GetType()
            .GetProperties()
            .Where(prop => !typeof(Delegate).IsAssignableFrom(prop.PropertyType))
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(options));

        string serializedData = _jsonSerializer.Serialize(properties);
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(serializedData));
        string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

        return hash;
    }

    

    public void Dispose()
    {
        if (_disposed) return;

        lock (_updateLock)
        {
            if (_disposed) return;
            _debounceTimer?.Dispose();
            _disposed = true;
        }
    }
}
