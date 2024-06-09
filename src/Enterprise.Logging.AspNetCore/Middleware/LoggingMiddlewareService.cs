using Enterprise.Logging.AspNetCore.Http;
using Enterprise.Logging.AspNetCore.Telemetry;
using Enterprise.Logging.AspNetCore.W3C;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Enterprise.Logging.AspNetCore.Middleware;

public static class LoggingMiddlewareService
{
    /// <summary>
    /// This registers the logging related services that are specific to an ASP.NET Core application.
    /// </summary>
    /// <param name="builder"></param>
    public static void RegisterLoggingMiddlewareServices(this IHostApplicationBuilder builder)
    {
        builder.ConfigureTelemetry();

        // These are for request logging.
        builder.Services.ConfigureHttpLogging(builder.Configuration);
        builder.Services.ConfigureW3CLogging(builder.Configuration);
    }

    /// <summary>
    /// Add the logging middleware to the request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public static void UseLogging(this WebApplication app)
    {
        LoggingMiddlewareOptions options = app.Services.GetRequiredService<IOptions<LoggingMiddlewareOptions>>().Value;

        app.UseHttpLogging();
        app.UseW3CLogging();

        options.UseProviders.Invoke(app);
    }
}
