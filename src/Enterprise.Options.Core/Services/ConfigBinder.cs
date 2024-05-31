using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Enterprise.Options.Core.Services;

internal static class ConfigBinder
{
    /// <summary>
    /// Binds a configuration section to a given options instance.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options class.</typeparam>
    /// <param name="configSectionKey">The key of the configuration section to bind.</param>
    /// <param name="config">The configuration instance.</param>
    /// <param name="options">The options instance to bind the configuration to.</param>
    public static void BindConfig<TOptions>(string? configSectionKey, IConfiguration config, TOptions options)
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
