using Enterprise.Api.Security.Options;
using Enterprise.Api.Swagger.Options;
using Enterprise.Monitoring.Health.Options;
using Enterprise.Options.Core.Singleton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.Options.Extensions;

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
        // Register the shared config options.
        services.RegisterOptions<SharedConfigOptions>(configuration, SharedConfigOptions.ConfigSectionKey);

        // Instantiate the shared options instance.
        // This will use the config provider to set initial values.
        SharedConfigOptions sharedConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<SharedConfigOptions>(configuration, SharedConfigOptions.ConfigSectionKey);

        SwaggerConfigOptions swaggerConfigOptions = new SwaggerConfigOptions();
        HealthCheckConfigOptions healthCheckConfigOptions = new HealthCheckConfigOptions();
        JwtBearerTokenOptions jwtBearerTokenOptions = new JwtBearerTokenOptions();

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
        OptionsInstanceService.Instance.ConfigureDefaultInstance(swaggerConfigOptions);
        OptionsInstanceService.Instance.ConfigureDefaultInstance(healthCheckConfigOptions);
        OptionsInstanceService.Instance.ConfigureDefaultInstance(jwtBearerTokenOptions);
    }
}