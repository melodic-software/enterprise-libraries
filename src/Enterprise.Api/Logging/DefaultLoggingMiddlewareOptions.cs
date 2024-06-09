using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Serilog.AspNetCore.Config;

namespace Enterprise.Api.Logging;

internal sealed class DefaultLoggingMiddlewareOptions : LoggingMiddlewareOptions
{
    internal DefaultLoggingMiddlewareOptions()
    {
        UseProviders = app =>
        {
            app.UseSerilog();
        };
    }
}
