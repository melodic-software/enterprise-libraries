using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Serialization.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Options;

/// <summary>
/// This dynamically monitors configuration options of a specific type.
/// This should be used for any options that need to be retrieved after the DI container has been created.
/// Use <see cref="OptionsInstanceService"/> when the application has not yet been built.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class DynamicOptionsMonitor<TOptions> : 
    IOptionsMonitor<TOptions>, 
    IOptionsUpdater<TOptions>, IDisposable
    where TOptions : class, new()
{
    private string _currentHash;
    private readonly IConfigurationSection? _configurationSection;
    private readonly ILogger<DynamicOptionsMonitor<TOptions>> _logger;
    private readonly TimeSpan _debouncePeriod;
    private readonly ISerializeJson _jsonSerializer;
    private readonly ChangeNotifier<TOptions> _changeNotifier;

    private readonly object _updateLock = new();
    private Timer? _debounceTimer;
    private bool _disposed;

    public TOptions CurrentValue { get; private set; }
    public TOptions Value => CurrentValue;

    public TOptions Get(string? name) => CurrentValue;
    TOptions IOptionsMonitor<TOptions>.Get(string? name) => CurrentValue;

    public DynamicOptionsMonitor(TOptions? currentValue,
        IConfigurationSection? configurationSection,
        ILogger<DynamicOptionsMonitor<TOptions>> logger,
        TimeSpan debouncePeriod,
        ISerializeJson jsonSerializer)
    {
        CurrentValue = currentValue ?? new TOptions();
        
        _configurationSection = configurationSection;
        _logger = logger;
        _debouncePeriod = debouncePeriod;
        _jsonSerializer = jsonSerializer;

        _changeNotifier = new ChangeNotifier<TOptions>(_logger);

        Type optionsType = typeof(TOptions);

        using (_logger.BeginScope("OptionsType: {OptionsType}", optionsType.Name))
        {
            try
            {
                if (_configurationSection == null)
                {
                    _currentHash = OptionsHashService.ComputeHash(CurrentValue, _jsonSerializer);
                    return;
                }

                ConfigBinder.Bind(CurrentValue, _configurationSection);

                _currentHash = OptionsHashService.ComputeHash(CurrentValue, _jsonSerializer);

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
        lock (_updateLock)
        {
            _logger.LogInformation("Adding configuration change listener.");
            // Return an IDisposable that removes the handler when disposed.
            return _changeNotifier.Subscribe(listener);
        }
    }

    public void UpdateOptions(Action<TOptions> applyChanges)
    {
        lock (_updateLock)
        {
            Type optionsType = typeof(TOptions);
            using (_logger.BeginScope("OptionsType: {OptionsType}", optionsType.Name))
            {
                _logger.LogInformation("Applying configuration changes.");
                applyChanges(CurrentValue);
                _changeNotifier.NotifySubscribers(CurrentValue);
                _logger.LogInformation("Configuration changes applied.");
            }
        }
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

            var newInstance = new TOptions();
            _configurationSection?.Bind(newInstance);

            // TODO: Consider performance optimizations here.
            // Computing and comparing hashes may be faster, particularly for large config objects.
            // Another option is to add derived types that specify specific properties and manually do the comparison.
            // We could also treat these as value objects and do equality checks without requiring serialization.
            // This might require some type constraints (via marker interfaces, etc.).

            string newHash = OptionsHashService.ComputeHash(newInstance, _jsonSerializer);
            bool configChanged = !string.Equals(_currentHash, newHash, StringComparison.Ordinal);

            if (configChanged)
            {
                _logger.LogInformation("Configuration section '{SectionName}' has changed. Reloading.", _configurationSection?.Path);
                
                CurrentValue = newInstance;
                
                _currentHash = newHash;
                _changeNotifier.NotifySubscribers(CurrentValue);
            }
            else
            {
                _logger.LogInformation("No relevant changes detected in the configuration section '{SectionName}'.", _configurationSection?.Path);
            }

            _logger.LogInformation("Configuration reloaded.");
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        lock (_updateLock)
        {
            if (_disposed)
            {
                return;
            }

            _debounceTimer?.Dispose();
            _disposed = true;
        }
    }
}
