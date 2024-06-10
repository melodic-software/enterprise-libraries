using Enterprise.Logging.AspNetCore.Middleware.Delegates;

namespace Enterprise.Logging.AspNetCore.Middleware;

public class LoggingMiddlewareOptions
{
    public const string ConfigSectionKey = "Custom:Logging:Middleware";

    /// <summary>
    /// Configure the specific provider available for use in the application.
    /// </summary>
    public UseProviders UseProviders { get; set; } = _ => { };
}
