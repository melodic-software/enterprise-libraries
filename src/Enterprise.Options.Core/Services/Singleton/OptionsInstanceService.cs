using System.Collections.Concurrent;
using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Options.Core.Delegates;
using Enterprise.Options.Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Enterprise.Options.Core.Services.Singleton;

/// <summary>
/// This configures and provides options instances.
/// It is only intended to be used during application startup, specifically before or during service registration.
/// This is primarily due to the fact that a service provider cannot be referenced until it has been built, which is after services have been configured and registered.
/// Using this service allows for providing options while configuring services with the DI container.
/// Once the web application has been built, the configure method should not be used.
/// </summary>
public class OptionsInstanceService
{
    private static readonly Lazy<OptionsInstanceService> Lazy = new(() => new OptionsInstanceService());
    public static OptionsInstanceService Instance => Lazy.Value;

    private ConcurrentDictionary<Type, ConfigureInitial> InitialConfigDictionary { get; } = new();
    private ConcurrentDictionary<Type, List<Configure>> AdditionalConfigDictionary { get; } = new();
    private ConcurrentDictionary<Type, OptionsInstanceDictionaryItem> InstanceDictionary { get; } = new();
    private ConcurrentDictionary<Type, CreateDefault> DefaultInstanceFactoryDictionary { get; } = new();

    /// <summary>
    /// This is to be used for testing purposes.
    /// It is a pragmatic way to allow for test separation, mitigating the testability issues with singleton implementations.
    /// This requires test project(s) to be granted visibility for internal members.
    /// </summary>
    internal static OptionsInstanceService CreateTestInstance() => new();

    private OptionsInstanceService() { }

    /// <summary>
    /// Configures a default options instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options class.</typeparam>
    /// <param name="instance">The options instance to configure as the default.</param>
    public void ConfigureDefaultInstance<TOptions>(TOptions instance) where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(instance);
        ConfigureDefaultInstance(() => instance);
    }

    /// <summary>
    /// Configure a default options instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options class.</typeparam>
    /// <param name="createDefault">The factory method to create the default options instance.</param>
    public void ConfigureDefaultInstance<TOptions>(CreateDefault<TOptions> createDefault) where TOptions : class, new()
    {
        Type type = typeof(TOptions);

        PreStartupLogger.Instance.LogInformation("Configuring default instance for type: {TypeName}.", type.Name);

        // Ensure only one default is registered at a time.
        if (DefaultInstanceFactoryDictionary.ContainsKey(type))
        {
            PreStartupLogger.Instance.LogInformation(
                "Default options instance factory already exists and will be replaced for type: {TypeName}.",
                type.Name
            );

            DefaultInstanceFactoryDictionary.TryRemove(type, out _);
        }

        DefaultInstanceFactoryDictionary.TryAdd(type, createDefault.Invoke);
        PreStartupLogger.Instance.LogDebug("Default options instance factory added for type: {TypeName}.", type.Name);
    }

    /// <summary>
    /// Configures options.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options class.</typeparam>
    /// <param name="configure">The configuration delegate.</param>
    /// <exception cref="InvalidOperationException">Thrown when attempting to reconfigure locked options.</exception>
    public void Configure<TOptions>(Configure<TOptions> configure) where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(configure);

        Type type = typeof(TOptions);

        PreStartupLogger.Instance.LogInformation("Configuring options for type: {TypeName}.", type.Name);

        bool isInitial = !InitialConfigDictionary.ContainsKey(type);

        if (isInitial)
        {
            InitialConfigDictionary.TryAdd(typeof(TOptions), ConfigureNew(configure));
            PreStartupLogger.Instance.LogDebug("Initial configuration added for type: {TypeName}.", type.Name);
        }
        else
        {
            bool containsAdditionalConfigurations = AdditionalConfigDictionary
                .TryGetValue(type, out List<Configure>? additionalConfigurations);

            additionalConfigurations ??= [];

            additionalConfigurations.Add(o =>
            {
                if (o is TOptions options)
                {
                    configure.Invoke(options);
                }
            });

            if (!containsAdditionalConfigurations)
            {
                AdditionalConfigDictionary.TryAdd(type, additionalConfigurations);
            }

            PreStartupLogger.Instance.LogDebug(
                "Additional configuration added for type: {TypeName}.",
                type.Name
            );
        }

        if (!InstanceDictionary.ContainsKey(type))
        {
            return;
        }

        InstanceDictionary.TryGetValue(type, out OptionsInstanceDictionaryItem? value);

        HandleLockedInstance(value, type);

        // Any config changes should clear the cached instance we have.
        // This will ensure the latest version of the configured options is used.
        InstanceDictionary.TryRemove(type, out OptionsInstanceDictionaryItem? removedValue);
    }

    /// <summary>
    /// Retrieves an options instance, creating and configuring it if necessary.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options class.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="configSectionKey">The key of the configuration section.</param>
    /// <returns>The configured options instance.</returns>
    /// <exception cref="Exception">Thrown when a type mismatch is found in the instance dictionary.</exception>
    public TOptions GetOptionsInstance<TOptions>(IConfiguration configuration, string? configSectionKey)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(configuration);

        Type type = typeof(TOptions);
        PreStartupLogger.Instance.LogDebug("Retrieving options instance for type: {TypeName}.", type.Name);

        if (InstanceDictionary.ContainsKey(type))
        {
            InstanceDictionary.TryGetValue(type, out OptionsInstanceDictionaryItem? value);

            TOptions options = value?.Options as TOptions ?? 
                throw new Exception($"Instance dictionary contains a type mismatch for the given key: {type}.");
            
            PreStartupLogger.Instance.LogDebug("Options instance found in cache for type: {TypeName}.", type);

            return options;
        }
        else
        {
            InitialConfigDictionary.TryGetValue(type, out ConfigureInitial? configureInitial);

            if (configureInitial == null)
            {
                PreStartupLogger.Instance.LogDebug(
                    "No initial configuration found. " +
                    "Creating new instance of type: {TypeName}.",
                    type.Name
                );
            }

            // This hasn't had any explicit configurations, so we're going to auto create and configure.
            configureInitial ??= Create<TOptions>;

            object? instance = configureInitial.Invoke(configuration, configSectionKey);
            TOptions options = instance as TOptions ?? new TOptions();

            AdditionalConfigDictionary.TryGetValue(type, out List<Configure>? additionalConfigurations);

            additionalConfigurations ??= [];

            foreach (Configure configure in additionalConfigurations)
            {
                configure.Invoke(options);
            }

            if (InstanceDictionary.ContainsKey(type))
            {
                return options;
            }

            var item = OptionsInstanceDictionaryItem.New(options);
            InstanceDictionary.TryAdd(type, item);

            return options;
        }
    }

    /// <summary>
    /// Use the specified options instance.
    /// This will lock the instance, meaning it can no longer be reconfigured with the singleton service.
    /// Updates to the config after this point must be done via an IOptionsUpdater&lt;T&gt;
    /// OR directly via DynamicOptionsMonitor&lt;T&gt;.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="options"></param>
    public void UseInstance<TOptions>(TOptions options)
        where TOptions : class, new()
    {
        ArgumentNullException.ThrowIfNull(options);

        Type type = typeof(TOptions);
        PreStartupLogger.Instance.LogInformation("Using instance for type: {TypeName}.", type.Name);

        if (InstanceDictionary.TryRemove(type, out _))
        {
            PreStartupLogger.Instance.LogInformation("Existing instance removed for type: {TypeName}.", type.Name);
        }

        InstanceDictionary.TryAdd(type, OptionsInstanceDictionaryItem.Locked(options));
        PreStartupLogger.Instance.LogInformation("New instance locked for type: {TypeName}.", type.Name);
    }

    private TOptions Create<TOptions>(IConfiguration config, string? configSectionKey)
        where TOptions : class, new()
    {
        TOptions options;

        if (DefaultInstanceFactoryDictionary.ContainsKey(typeof(TOptions)))
        {
            DefaultInstanceFactoryDictionary.TryGetValue(typeof(TOptions), out CreateDefault? createDefault);
            options = createDefault?.Invoke() as TOptions ?? new TOptions();
        }
        else
        {
            options = new TOptions();
        }

        ConfigBinder.BindConfig(configSectionKey, config, options);

        return options;
    }

    private ConfigureInitial ConfigureNew<TOptions>(Configure<TOptions> configure)
        where TOptions : class, new()
    {
        return (config, configSectionKey) =>
        {
            TOptions options = Create<TOptions>(config, configSectionKey);
            configure.Invoke(options);
            return options;
        };
    }

    private static void HandleLockedInstance(OptionsInstanceDictionaryItem? dictionaryItem, MemberInfo type)
    {
        if (dictionaryItem is not { IsLocked: true })
        {
            return;
        }

        // This is likely due to the options being monitored externally.
        // This class is only meant to be used on application startup when a service provider is not available.

        PreStartupLogger.Instance.LogError(
            "Attempted to reconfigure locked options for type: {TypeName}.", type.Name
        );

        throw new InvalidOperationException(
            "A locked options instance has already been registered and can no longer be reconfigured."
        );
    }
}
