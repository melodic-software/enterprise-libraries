using Enterprise.Logging.Providers.Delegates;

namespace Enterprise.Logging.Providers;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/logging-providers
// https://learn.microsoft.com/en-us/dotnet/core/extensions/logging-providers#built-in-logging-providers

public class LoggingProviderOptions
{
    public const string ConfigSectionKey = "Custom:Logging:Providers";

    /// <summary>
    /// This is useful in local environments but also in cloud environments where console output is captured.
    /// Containerized applications will also have log output.
    /// https://learn.microsoft.com/en-us/dotnet/core/extensions/console-log-formatter
    /// </summary>
    public bool EnableConsole { get; set; } = true;
    public bool EnableJsonConsole { get; set; } = true;
    public bool EnableAddSystemdConsole { get; set; }
    public bool EnableDebug { get; set; } = true;
    public bool EnableEventSource { get; set; } = true;
    public bool EnableEventLog { get; set; } = true;
    
    public bool EnableApplicationInsights { get; set; }

    /// <summary>
    /// Configure the specific provider service(s) available for use in the application.
    /// </summary>
    public ConfigureProviders ConfigureProviders { get; set; } = _ => { };
}
