using Enterprise.Api.Controllers.Options;
using Enterprise.Api.ErrorHandling.Options;
using Enterprise.Api.Minimal.Options;
using Enterprise.Api.Security.Options;
using Enterprise.Api.SignalR.Options;
using Enterprise.Api.Swagger.Options;
using Enterprise.Api.Versioning.Options;
using Enterprise.AutoMapper.Options;
using Enterprise.Cors.Options;
using Enterprise.Hosting.AspNetCore.Options;
using Enterprise.Logging.AspNetCore.Http;
using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Logging.AspNetCore.Telemetry;
using Enterprise.Logging.AspNetCore.W3C;
using Enterprise.Logging.Options;
using Enterprise.Logging.Providers;
using Enterprise.Logging.TraceListeners;
using Enterprise.MediatR.Options;
using Enterprise.Monitoring.Health.Options;
using Enterprise.Multitenancy.Options;
using Enterprise.Options.Core.Delegates;
using Enterprise.Quartz.Options;
using Enterprise.Redis.Options;
using Enterprise.Serilog.Options;
using Enterprise.Traceability.Options;

namespace Enterprise.Api.Startup.Options;

public static class WebApiOptionsExtensions
{
    public static void ConfigureAutoMapper(this WebApiOptions options, Configure<AutoMapperOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureControllers(this WebApiOptions options, Configure<ControllerOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureCors(this WebApiOptions options, Configure<CorsOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureErrorHandling(this WebApiOptions options, Configure<ErrorHandlingOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureHealthChecks(this WebApiOptions options, Configure<HealthCheckOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureHttpLogging(this WebApiOptions options, Configure<HttpLoggingOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureIISIntegration(this WebApiOptions options, Configure<IISIntegrationOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureJwtBearerTokenOptions(this WebApiOptions options, Configure<JwtBearerTokenOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureLogging(this WebApiOptions options, Configure<LoggingOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureLoggingProviders(this WebApiOptions options, Configure<LoggingProviderOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureLoggingMiddleware(this WebApiOptions options, Configure<LoggingMiddlewareOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureMediatR(this WebApiOptions options, Configure<MediatROptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureMinimalApi(this WebApiOptions options, Configure<MinimalApiOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureMultitenancy(this WebApiOptions options, Configure<MultitenancyOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureQuartz(this WebApiOptions options, Configure<QuartzOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureRedis(this WebApiOptions options, Configure<RedisOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureSerilog(this WebApiOptions options, Configure<SerilogOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureSignalR(this WebApiOptions options, Configure<SignalROptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureSwagger(this WebApiOptions options, Configure<SwaggerOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureSwaggerSecurity(this WebApiOptions options, Configure<SwaggerSecurityOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureSwaggerUI(this WebApiOptions options, Configure<SwaggerUIOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureTelemetry(this WebApiOptions options, Configure<TelemetryOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureTraceability(this WebApiOptions options, Configure<OpenTelemetryOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureTraceListeners(this WebApiOptions options, Configure<TraceListenerOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureVersioning(this WebApiOptions options, Configure<VersioningOptions> configure)
    {
        options.Configure(configure);
    }

    public static void ConfigureW3CLogging(this WebApiOptions options, Configure<W3CLoggingOptions> configure)
    {
        options.Configure(configure);
    }
}
