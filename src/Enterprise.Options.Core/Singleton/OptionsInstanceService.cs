using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Enterprise.Options.Core.Singleton;

/// <summary>
/// This configures and provides custom options instances.
/// It is only intended to be used during application startup, specifically before or during service registration.
/// This is primarily due to the fact that a service provider cannot be referenced to provide options via DI.
/// Once the web application has been built, the configure method should not be used.
/// </summary>
public class OptionsInstanceService
{
    private static readonly Lazy<OptionsInstanceService> _lazy = new(() => new OptionsInstanceService());
    public static OptionsInstanceService Instance => _lazy.Value;

    private ConcurrentDictionary<Type, Func<IConfiguration, string?, object>> InitialDelegateDictionary { get; } = new();
    private ConcurrentDictionary<Type, List<Action<object>>> AdditionalActionDictionary { get; } = new();
    private ConcurrentDictionary<Type, OptionsInstanceDictionaryItem> InstanceDictionary { get; } = new();
    private ConcurrentDictionary<Type, Func<object>> DefaultInstanceFactoryDictionary { get; } = new();

    private OptionsInstanceService()
    {

    }

    public void ConfigureDefaultInstance<TOptions>(TOptions instance) where TOptions : class, new()
    {
        ConfigureDefaultInstance(() => instance);
    }

    public void ConfigureDefaultInstance<TOptions>(Func<TOptions> createDefault) where TOptions : class, new()
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

        DefaultInstanceFactoryDictionary.TryAdd(type, createDefault);
        PreStartupLogger.Instance.LogDebug("Default options instance factory added for type: {TypeName}.", type.Name);
    }

    public void Configure<TOptions>(Action<TOptions> action) where TOptions : class, new()
    {
        Type type = typeof(TOptions);
        PreStartupLogger.Instance.LogInformation("Configuring options for type: {TypeName}.", type.Name);

        bool isInitial = !InitialDelegateDictionary.ContainsKey(type);

        if (isInitial)
        {
            InitialDelegateDictionary.TryAdd(typeof(TOptions), ConfigureNew(action));
            PreStartupLogger.Instance.LogDebug("Initial configuration action added for type: {TypeName}.", type.Name);
        }
        else
        {
            bool containsActions = AdditionalActionDictionary.TryGetValue(type, out List<Action<object>>? actions);

            actions ??= [];

            actions.Add(o =>
            {
                if (o is TOptions options)
                {
                    action.Invoke(options);
                }
            });

            if (!containsActions)
            {
                AdditionalActionDictionary.TryAdd(type, actions);
            }

            PreStartupLogger.Instance.LogDebug("Additional configuration action added for type: {TypeName}.", type.Name);
        }

        if (!InstanceDictionary.ContainsKey(type))
        {
            return;
        }

        InstanceDictionary.TryGetValue(type, out OptionsInstanceDictionaryItem? value);

        if (value is { IsLocked: true })
        {
            // This is likely due to the options being monitored externally.
            // This class is only meant to be used on application startup when a service provider is not available.
            PreStartupLogger.Instance.LogError("Attempted to reconfigure locked options for type: {TypeName}.", type.Name);

            throw new InvalidOperationException(
                "A locked options instance has already been registered and can no longer be reconfigured."
            );
        }

        // Any config changes should clear the cached instance we have.
        // This will ensure the latest version of the configured options is used.
        InstanceDictionary.TryRemove(type, out OptionsInstanceDictionaryItem? removedValue);
    }

    public TOptions GetOptionsInstance<TOptions>(IConfiguration configuration, string? configSectionKey) where TOptions : class, new()
    {
        Type type = typeof(TOptions);
        PreStartupLogger.Instance.LogDebug("Retrieving options instance for type: {TypeName}.", type.Name);

        if (InstanceDictionary.ContainsKey(type))
        {
            InstanceDictionary.TryGetValue(type, out OptionsInstanceDictionaryItem? value);
            TOptions options = value?.Options as TOptions ?? throw new Exception($"Instance dictionary contains a type mismatch for the given key: {type}.");
            PreStartupLogger.Instance.LogDebug("Options instance found in cache for type: {TypeName}.", type);
            return options;
        }
        else
        {
            InitialDelegateDictionary.TryGetValue(type, out Func<IConfiguration, string?, object>? func);

            if (func == null)
            {
                PreStartupLogger.Instance.LogDebug("No initial configuration found. Creating new instance of type: {TypeName}.", type.Name);
            }

            // This hasn't had any explicit configurations, so we're going to auto create and configure.
            func ??= Create<TOptions>;

            object? instance = func.Invoke(configuration, configSectionKey);
            TOptions options = instance as TOptions ?? new TOptions();

            AdditionalActionDictionary.TryGetValue(type, out List<Action<object>>? actions);

            actions ??= [];

            var typedActions = actions.OfType<Action<TOptions>>().ToList();

            foreach (Action<TOptions> action in typedActions)
            {
                action.Invoke(options);
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

    public void UseInstance<TOptions>(TOptions options) where TOptions : class, new()
    {
        Type type = typeof(TOptions);
        PreStartupLogger.Instance.LogInformation("Using instance for type: {TypeName}.", type.Name);

        if (InstanceDictionary.TryRemove(type, out _))
        {
            PreStartupLogger.Instance.LogInformation("Existing instance removed for type: {TypeName}.", type.Name);
        }

        InstanceDictionary.TryAdd(type, OptionsInstanceDictionaryItem.Locked(options));
        PreStartupLogger.Instance.LogInformation("New instance locked for type: {TypeName}.", type.Name);
    }

    private Func<IConfiguration, string?, object> ConfigureNew<TOptions>(Action<TOptions> configure) where TOptions : class, new()
    {
        return (config, configSectionKey) =>
        {
            TOptions options = Create<TOptions>(config, configSectionKey);
            configure.Invoke(options);
            return options;
        };
    }

    private TOptions Create<TOptions>(IConfiguration config, string? configSectionKey) where TOptions : class, new()
    {
        TOptions options;

        if (DefaultInstanceFactoryDictionary.ContainsKey(typeof(TOptions)))
        {
            DefaultInstanceFactoryDictionary.TryGetValue(typeof(TOptions), out Func<object>? defaultInstanceFactory);
            options = defaultInstanceFactory?.Invoke() as TOptions ?? new TOptions();
        }
        else
        {
            options = new TOptions();
        }

        BindConfig(configSectionKey, config, options);
        return options;
    }

    private void BindConfig<TOptions>(string? configSectionKey, IConfiguration config, TOptions options)
        where TOptions : class, new()
    {
        IConfigurationSection? configSection = !string.IsNullOrWhiteSpace(configSectionKey)
            ? config.GetSection(configSectionKey)
            : null;

        if (configSection != null)
        {
            PreStartupLogger.Instance.LogDebug(
                "Binding configuration for section \"{ConfigSectionKey}\" to type: {OptionsTypeName}.",
                configSectionKey,
                typeof(TOptions).Name
            );

            // This will initialize any property values with those found in the config section.   
            configSection.Bind(options);
        }
        else
        {
            PreStartupLogger.Instance.LogDebug(
                "No configuration section specified for binding to type: {OptionsTypeName}. Using default values.",
                typeof(TOptions).Name
            );
        }
    }
}
