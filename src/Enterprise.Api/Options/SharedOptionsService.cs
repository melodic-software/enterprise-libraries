using Enterprise.Api.Security.Options;
using Enterprise.Api.Swagger.Options;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Monitoring.Health.Options;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Options;

internal static class SharedOptionsService
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
        services.RegisterOptions<SharedOptions>(configuration, SharedOptions.ConfigSectionKey);

        // This will use the config provider to set initial values.
        SharedOptions sharedOptions = GetOptionsInstance<SharedOptions>(configuration, SharedOptions.ConfigSectionKey);
        SwaggerOptions swaggerOptions = GetOptionsInstance<SwaggerOptions>(configuration, SwaggerOptions.ConfigSectionKey);
        SwaggerSecurityOptions swaggerSecurityOptions = GetOptionsInstance<SwaggerSecurityOptions>(configuration, SwaggerSecurityOptions.ConfigSectionKey);
        HealthCheckOptions healthCheckOptions = GetOptionsInstance<HealthCheckOptions>(configuration, HealthCheckOptions.ConfigSectionKey);
        JwtBearerTokenOptions jwtBearerTokenOptions = GetOptionsInstance<JwtBearerTokenOptions>(configuration, JwtBearerTokenOptions.ConfigSectionKey);

        if (!string.IsNullOrWhiteSpace(sharedOptions.ApplicationDisplayName))
        {
            swaggerOptions.ApplicationName = sharedOptions.ApplicationDisplayName;
        }

        if (!string.IsNullOrWhiteSpace(sharedOptions.ApplicationDescription))
        {
            swaggerOptions.ApplicationDescription = sharedOptions.ApplicationDescription;
        }

        if (!string.IsNullOrWhiteSpace(sharedOptions.OAuthAuthority))
        {
            swaggerSecurityOptions.Authority = sharedOptions.OAuthAuthority;
            healthCheckOptions.OpenIdConnectAuthority = sharedOptions.OAuthAuthority;
            jwtBearerTokenOptions.Authority = sharedOptions.OAuthAuthority;
        }

        // Configure default instances.
        OptionsInstanceService.Instance.ConfigureDefaultInstance(healthCheckOptions);
        OptionsInstanceService.Instance.ConfigureDefaultInstance(jwtBearerTokenOptions);
        OptionsInstanceService.Instance.ConfigureDefaultInstance(swaggerOptions);
        OptionsInstanceService.Instance.ConfigureDefaultInstance(swaggerSecurityOptions);
    }

    private static T GetOptionsInstance<T>(IConfiguration configuration, string configSectionKey) where T : class, new() =>
        OptionsInstanceService.Instance.GetOptionsInstance<T>(configuration, configSectionKey);
}
