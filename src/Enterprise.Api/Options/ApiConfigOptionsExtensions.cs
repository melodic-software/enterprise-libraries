using Enterprise.Api.Controllers.Options;
using Enterprise.Api.ErrorHandling.Options;
using Enterprise.Api.Minimal.Options;
using Enterprise.Api.Security.Options;
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
using Enterprise.Quartz.Options;
using Enterprise.Redis.Options;
using Enterprise.Serilog.Options;
using Enterprise.Traceability.Options;

namespace Enterprise.Api.Options;

public static class ApiConfigOptionsExtensions
{
    public static void ConfigureAutoMapper(this ApiConfigOptions options, Action<AutoMapperConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureControllers(this ApiConfigOptions options, Action<ControllerConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureCors(this ApiConfigOptions options, Action<CorsConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureErrorHandling(this ApiConfigOptions options, Action<ErrorHandlingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureHealthChecks(this ApiConfigOptions options, Action<HealthCheckConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureHttpLogging(this ApiConfigOptions options, Action<HttpLoggingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureIISIntegration(this ApiConfigOptions options, Action<IISIntegrationOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureJwtBearerTokenOptions(this ApiConfigOptions options, Action<JwtBearerTokenOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureLogging(this ApiConfigOptions options, Action<LoggingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureLoggingProviders(this ApiConfigOptions options, Action<LoggingProviderConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureLoggingMiddleware(this ApiConfigOptions options, Action<LoggingMiddlewareConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureMediatR(this ApiConfigOptions options, Action<MediatRConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureMinimalApi(this ApiConfigOptions options, Action<MinimalApiConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureMultitenancy(this ApiConfigOptions options, Action<MultitenancyConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureQuartz(this ApiConfigOptions options, Action<QuartzConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureRedis(this ApiConfigOptions options, Action<RedisConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureSerilog(this ApiConfigOptions options, Action<SerilogConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureSwagger(this ApiConfigOptions options, Action<SwaggerConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureTelemetry(this ApiConfigOptions options, Action<TelemetryConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureTraceability(this ApiConfigOptions options, Action<OpenTelemetryConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureTraceListeners(this ApiConfigOptions options, Action<TraceListenerConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureVersioning(this ApiConfigOptions options, Action<VersioningConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureW3CLogging(this ApiConfigOptions options, Action<W3CLoggingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }
}
