using static Enterprise.Logging.Options.LoggingOptionDefaults;

namespace Enterprise.Logging.AspNetCore.W3C;

public class W3CLoggingOptions
{
    public const string ConfigSectionKey = "Custom:Logging:Middleware:W3C";

    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/w3c-logger
    /// W3CLogger is a middleware that writes log files in the W3C standard format. The logs contain information about HTTP requests and HTTP responses.
    /// It can reduce the performance of an app, especially when logging the request and response bodies.
    /// It can potentially log personally identifiable information (PII). Consider the risk and avoid logging sensitive information.
    /// </summary>
    public bool EnableW3CLogging { get; set; }

    /// <summary>
    /// This is the name of the application that will be included in the log file name.
    /// Example: {ApplicationName}-W3C20230906.0000.txt
    /// This must be set if W3C logging is enabled.
    /// </summary>
    public string W3CLogFileApplicationName { get; set; } = LogFileApplicationName;
}
