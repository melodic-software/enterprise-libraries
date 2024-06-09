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
using Enterprise.Options.Core.Delegates;
using Enterprise.Quartz.Options;
using Enterprise.Redis.Options;
using Enterprise.Serilog.Options;
using Enterprise.Traceability.Options;

namespace Enterprise.Api.Startup.Options;

public static class ApiConfigOptionsExtensions
{
    public static void ConfigureAutoMapper(this ApiConfigOptions options, Configure<AutoMapperConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureControllers(this ApiConfigOptions options, Configure<ControllerConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureCors(this ApiConfigOptions options, Configure<CorsConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureErrorHandling(this ApiConfigOptions options, Configure<ErrorHandlingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureHealthChecks(this ApiConfigOptions options, Configure<HealthCheckConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureHttpLogging(this ApiConfigOptions options, Configure<HttpLoggingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureIISIntegration(this ApiConfigOptions options, Configure<IISIntegrationOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureJwtBearerTokenOptions(this ApiConfigOptions options, Configure<JwtBearerTokenOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureLogging(this ApiConfigOptions options, Configure<LoggingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureLoggingProviders(this ApiConfigOptions options, Configure<LoggingProviderConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureLoggingMiddleware(this ApiConfigOptions options, Configure<LoggingMiddlewareConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureMediatR(this ApiConfigOptions options, Configure<MediatRConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureMinimalApi(this ApiConfigOptions options, Configure<MinimalApiConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureMultitenancy(this ApiConfigOptions options, Configure<MultitenancyConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureQuartz(this ApiConfigOptions options, Configure<QuartzConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureRedis(this ApiConfigOptions options, Configure<RedisConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureSerilog(this ApiConfigOptions options, Configure<SerilogConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureSwagger(this ApiConfigOptions options, Configure<SwaggerConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureTelemetry(this ApiConfigOptions options, Configure<TelemetryConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureTraceability(this ApiConfigOptions options, Configure<OpenTelemetryConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureTraceListeners(this ApiConfigOptions options, Configure<TraceListenerConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureVersioning(this ApiConfigOptions options, Configure<VersioningConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }

    public static void ConfigureW3CLogging(this ApiConfigOptions options, Configure<W3CLoggingConfigOptions> configureOptions)
    {
        options.Configure(configureOptions);
    }
}
