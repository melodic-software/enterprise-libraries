using Microsoft.AspNetCore.Builder;

namespace Enterprise.Logging.AspNetCore.Options;

public class LoggingMiddlewareConfigOptions
{
    public const string ConfigSectionKey = "Custom:Logging:Middleware";

    /// <summary>
    /// Configure the specific provider available for use in the application.
    /// </summary>
    public Action<WebApplication> UseProviders = _ => { };
}