using Enterprise.Cors.Constants;
using Enterprise.Cors.Options;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Enterprise.Cors.Config.CorsConfigurations;

namespace Enterprise.Cors.Config;

public static class CorsConfigService
{
    /// <summary>
    /// Configure CORS (Cross-Origin Resource Sharing).
    /// This is a mechanism to give or restrict access rights to applications from different domains.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="environment"></param>
    /// <param name="configuration"></param>
    /// <exception cref="Exception"></exception>
    public static void ConfigureCors(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        CorsOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<CorsOptions>(configuration, CorsOptions.ConfigSectionKey);

        if (!options.EnableCors)
        {
            return;
        }

        if (options.CustomConfigure != null)
        {
            // Allow for full customization
            services.AddCors(options.CustomConfigure);
            return;
        }

        if (!environment.IsProduction())
        {
            // This should only be used in pre-production environments
            services.AddCors(RelaxedCorsConfiguration);
            return;
        }

        string policyName = CorsPolicyNames.DefaultPolicyName;

        string[] allowedOrigins = options.AllowedOrigins
            .Select(x => x.Trim())
            .Distinct()
            .ToArray();

        if (!allowedOrigins.Any())
        {
            PreStartupLogger.Instance.LogError("CORS has been enabled, but no origins are allowed.");
        }

        if (allowedOrigins.Any(x => x == "*"))
        {
            throw new Exception("The wildcard origin \"*\" cannot be used in production. It is not secure.");
        }

        services.AddCors(corsOptions =>
        {
            StandardCorsConfiguration(policyName, corsOptions, options);
        });
    }

    public static void UseCors(this WebApplication app)
    {
        CorsOptions options = app.Services.GetRequiredService<IOptions<CorsOptions>>().Value;

        if (options.EnableCors)
        {
            app.UseCors(CorsPolicyNames.DefaultPolicyName);
        }
    }
}
