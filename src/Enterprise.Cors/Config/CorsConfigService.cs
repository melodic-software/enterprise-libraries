using Enterprise.Cors.Constants;
using Enterprise.Cors.Options;
using Enterprise.Cors.Policies;
using Enterprise.Options.Core.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
        CorsConfigOptions options = OptionsInstanceService.Instance.GetOptionsInstance<CorsConfigOptions>(configuration, CorsConfigOptions.ConfigSectionKey);

        if (!options.EnableCors)
            return;

        if (options.ConfigureCustom != null)
        {
            // allow for full customization
            services.AddCors(options.ConfigureCustom);
            return;
        }

        if (!environment.IsProduction())
        {
            // this should only be used in pre-production environments
            services.AddCors(RelaxedCorsConfiguration);
            return;
        }

        string policyName = CorsPolicyNames.DefaultPolicyName;
        string[] allowedOrigins = options.AllowedOrigins.Distinct().ToArray();

        // TODO: add logging (warning) here instead of throwing an exception
        // the logger may not be available until the application is built, so a collection of log messages may need to be instantiated
        // or a logger reference may need to be instantiated elsewhere and referenced here
        if (!allowedOrigins.Any())
            throw new Exception("CORS has been enabled, but no origins are allowed.");

        services.AddCors(corsOptions =>
        {
            StandardCorsConfiguration(corsOptions, policyName, allowedOrigins);
        });
    }

    public static void UseCors(this WebApplication app)
    {
        CorsConfigOptions corsConfigOptions = app.Services.GetRequiredService<IOptions<CorsConfigOptions>>().Value;

        if (corsConfigOptions.EnableCors)
            app.UseCors(CorsPolicyNames.DefaultPolicyName);
    }

    /// <summary>
    /// This allows any origin, HTTP method or header. It should not be used in production environments.
    /// </summary>
    public static Action<CorsOptions> RelaxedCorsConfiguration => options =>
    {
        string policyName = CorsPolicyNames.RelaxedPolicyName;

        if (CorsPolicyService.PolicyExists(options, policyName))
            return;

        options.AddPolicy(policyName, corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    };

    public static Action<CorsOptions, string, string[]> StandardCorsConfiguration => (options, policyName, allowedOrigins) =>
    {
        if (CorsPolicyService.PolicyExists(options, policyName))
            return;

        options.AddPolicy(policyName, corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    };
}