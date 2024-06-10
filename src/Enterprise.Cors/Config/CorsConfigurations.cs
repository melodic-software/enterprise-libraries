using Enterprise.Cors.Config.Delegates;
using Enterprise.Cors.Constants;
using Enterprise.Cors.Policies;
using Enterprise.Options.Core.Delegates;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.Cors.Config;

public static class CorsConfigurations
{
    /// <summary>
    /// This allows any origin, HTTP method or header. It should not be used in production environments.
    /// </summary>
    public static Configure<CorsOptions> RelaxedCorsConfiguration => options =>
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

    public static ConfigureCors StandardCorsConfiguration => (options, policyName, allowedOrigins) =>
    {
        if (CorsPolicyService.PolicyExists(options, policyName))
        {
            return;
        }

        options.AddPolicy(policyName, corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    };
}
