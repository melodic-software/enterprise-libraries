using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Serilog.AspNetCore.Config;

namespace Enterprise.Api.Logging;

internal sealed class DefaultLoggingMiddlewareConfigOptions : LoggingMiddlewareConfigOptions
{
    internal DefaultLoggingMiddlewareConfigOptions()
    {
        UseProviders = app =>
        {
            app.UseSerilog();
        };
    }
}
