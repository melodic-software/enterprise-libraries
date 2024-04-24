using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Options;

public class LoggingConfigOptions
{
    public const string ConfigSectionKey = "Custom:Logging";

    /// <summary>
    /// A custom configuration delegate for configuring log filters (in code).
    /// It is recommended to use configuration (appSettings.json) over in code configuration.
    /// </summary>
    public Action<ILoggingBuilder>? AddLogFilters { get; set; } = null;

    /// <summary>
    /// Completely customize logging configuration.
    /// If this is provided, all the logging configuration defaults will not be configured.
    /// </summary>
    public Action<ILoggingBuilder>? CustomConfigureLogging { get; set; } = null;

    /// <summary>
    /// This is used internally in the logging configuration setup.
    /// It is an extension point that can be used to register additional services.
    /// </summary>
    public Action<IHostApplicationBuilder>? ConfigureExtendedServices { get; set; } = null;
}