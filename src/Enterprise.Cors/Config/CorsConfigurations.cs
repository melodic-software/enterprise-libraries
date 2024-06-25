using Enterprise.Cors.Config.Delegates;
using Enterprise.Cors.Constants;
using Enterprise.Cors.Policies;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.Cors.Config;

public static class CorsConfigurations
{
    /// <summary>
    /// This allows any origin, HTTP method or header. It should not be used in production environments.
    /// </summary>
    public static Action<CorsOptions> RelaxedCorsConfiguration => options =>
    {
        string policyName = CorsPolicyNames.RelaxedPolicyName;

        if (CorsPolicyService.PolicyExists(options, policyName))
        {
            return;
        }

        options.AddPolicy(policyName, corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    };

    public static ConfigureCors StandardCorsConfiguration => (policyName, options, customOptions) =>
    {
        if (CorsPolicyService.PolicyExists(options, policyName))
        {
            return;
        }

        string[] allowedOrigins = customOptions.AllowedOrigins.Select(x => x.Trim()).Distinct().ToArray();
        string[] allowedMethods = customOptions.AllowedMethods.Select(x => x.Trim()).Distinct().ToArray();
        string[] allowedHeaders = customOptions.AllowedHeaders.Select(x => x.Trim()).Distinct().ToArray();
        string[] exposedHeaders = customOptions.ExposedHeaders.Select(x => x.Trim()).Distinct().ToArray();

        bool noWildcard = allowedOrigins.Any(x => x == "*");
        bool allowCredentials = customOptions.AllowCredentials && noWildcard;

        options.AddPolicy(policyName, corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(allowedOrigins);

            if (allowedMethods.Any())
            {
                corsPolicyBuilder.WithMethods(allowedMethods);
            }
            else
            {
                corsPolicyBuilder.AllowAnyMethod();
            }

            if (allowedHeaders.Any())
            {
                corsPolicyBuilder.WithHeaders(allowedHeaders);
            }
            else
            {
                corsPolicyBuilder.AllowAnyHeader();
            }

            if (exposedHeaders.Any())
            {
                corsPolicyBuilder.WithExposedHeaders(exposedHeaders);
            }

            if (allowCredentials)
            {
                corsPolicyBuilder.AllowCredentials();
            }
            else
            {
                corsPolicyBuilder.DisallowCredentials();
            }
        });
    };
}
