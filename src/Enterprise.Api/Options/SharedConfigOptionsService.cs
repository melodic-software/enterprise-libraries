using Enterprise.Api.Security.Options;
using Enterprise.Api.Swagger.Options;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Monitoring.Health.Options;
using Enterprise.Options.Core.Singleton;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Options;

internal static class SharedConfigOptionsService
{
    /// <summary>
    /// Use the shared config to set or override specific values.
    /// This enables setting multiple dependent config values with a singular set of config entries.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    internal static void ConfigureShared(IServiceCollection services, IConfiguration configuration)
    {
        PreStartupLogger.Instance.LogInformation("Applying shared configuration.");

        // Register the shared config options.
        services.RegisterOptions<SharedConfigOptions>(configuration, SharedConfigOptions.ConfigSectionKey);

        // This will use the config provider to set initial values.
        SharedConfigOptions sharedConfigOptions = GetOptionsInstance<SharedConfigOptions>(configuration, SharedConfigOptions.ConfigSectionKey);
        SwaggerConfigOptions swaggerConfigOptions = GetOptionsInstance<SwaggerConfigOptions>(configuration, SwaggerConfigOptions.ConfigSectionKey);
        HealthCheckConfigOptions healthCheckConfigOptions = GetOptionsInstance<HealthCheckConfigOptions>(configuration, HealthCheckConfigOptions.ConfigSectionKey);
        JwtBearerTokenOptions jwtBearerTokenOptions = GetOptionsInstance<JwtBearerTokenOptions>(configuration, JwtBearerTokenOptions.ConfigSectionKey);

        if (!string.IsNullOrWhiteSpace(sharedConfigOptions.ApplicationDisplayName))
        {
            swaggerConfigOptions.ApplicationName = sharedConfigOptions.ApplicationDisplayName;
        }

        if (!string.IsNullOrWhiteSpace(sharedConfigOptions.OAuthAuthority))
        {
            swaggerConfigOptions.Authority = sharedConfigOptions.OAuthAuthority;
            healthCheckConfigOptions.OpenIdConnectAuthority = sharedConfigOptions.OAuthAuthority;
            jwtBearerTokenOptions.Authority = sharedConfigOptions.OAuthAuthority;
        }

        // Configure default instances.
        OptionsInstanceService.Instance.ConfigureDefaultInstance(healthCheckConfigOptions);
        OptionsInstanceService.Instance.ConfigureDefaultInstance(jwtBearerTokenOptions);
        OptionsInstanceService.Instance.ConfigureDefaultInstance(swaggerConfigOptions);
    }

    private static T GetOptionsInstance<T>(IConfiguration configuration, string configSectionKey) where T : class, new() =>
        OptionsInstanceService.Instance.GetOptionsInstance<T>(configuration, configSectionKey);
}
