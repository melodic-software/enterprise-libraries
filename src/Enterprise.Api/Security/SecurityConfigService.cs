using Enterprise.Api.Middleware;
using Enterprise.Api.Security.Authorization.Options;
using Enterprise.Api.Security.OAuth.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;
using static Enterprise.Api.Security.Constants.SecurityConstants;

namespace Enterprise.Api.Security;

public static class SecurityConfigService
{
    public static void ConfigureSecurity(this IServiceCollection services, IHostApplicationBuilder builder)
    {
        if (SkipConfiguration(builder.Configuration, builder.Environment))
        {
            return;
        }

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        AddAuthentication(builder, services);
        AddAuthorization(services);
    }

    public static void UseSecurity(this WebApplication app)
    {
        if (SkipConfiguration(app.Configuration, app.Environment))
        {
            return;
        }

        app.UseAuthentication();
        app.UseMiddleware<UserLoggingScopeMiddleware>();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            // Some errors may be obfuscated by this.
            // Do NOT enable this in production.
            // PII = Personally Identifiable Information.
            IdentityModelEventSource.ShowPII = true;
        }
    }

    private static bool SkipConfiguration(IConfiguration configuration, IHostEnvironment environment)
    {
        bool isNotProduction = !environment.IsProduction();
        bool disableSecurity = configuration.GetValue("DisableSecurity", false);
        bool skipConfiguration = isNotProduction && disableSecurity;
        return skipConfiguration;
    }

    private static void AddAuthentication(IHostApplicationBuilder builder, IServiceCollection services)
    {
        // This builder sets up the authentication related services.
        AuthenticationBuilder authBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = DefaultAuthenticationScheme;
            options.DefaultChallengeScheme = DefaultAuthenticationScheme;
            options.DefaultScheme = DefaultAuthenticationScheme;
        });

        //authBuilder.AddBasicAuthentication();
        authBuilder.AddJwtBearer(builder.Environment, builder.Configuration);
        //authBuilder.AddOAuth2Introspection(authority: jwtBearerTokenOptions.Authority);
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        AuthorizationBuilder authBuilder = services.AddAuthorizationBuilder();

        // These policy names can be assigned to the Policy property on Authorize attributes on a controller class or method.
        // Requests that do not fulfill the policy will return a 403 "forbidden" status code.
        authBuilder.AddPolicy(DefaultAuthPolicyName, policy =>
        {
            policy.RequireAuthenticatedUser();
            // Add any additional policy rules here.
        });

        // TODO: add delegate to add custom authorization policies
        // https://app.pluralsight.com/course-player?clipId=6f802090-fb1c-4618-b1ae-fbb54c13d789

        authBuilder.AddDefaultPolicy();
        authBuilder.AddDefaultFallbackPolicy();

        // TODO: Allow for application specific policies like this:
        // There are a few different options for how this can be implemented.
        // 1) Add a nullable Func<AuthorizationBuilder> to the options facade object(s).
        // 2) Add an object array to the options with a name and a Func<AuthorizationPolicyBuilder>.
        // 3) Use DI and auto wire up using an interface like "IAddPolicy".
        //authBuilder.AddPolicy("RequireRoleAndClaim", policyBuilder =>
        //    policyBuilder
        //        .RequireRole("admin")
        //        .RequireClaim("country", "Belgium"));

        // https://app.pluralsight.com/course-player?clipId=7a3e77a2-2a7c-4177-825b-a51009cb36b7

        // Authorization API
        // https://app.pluralsight.com/course-player?clipId=c734bcbd-061e-493e-81bb-fe1e258ee652
        // Take a look at the token exchange (a custom grant type extension implemented in IdentityServer)
        // if a separate authorization API is used, and you don't want to reuse access tokens between APIs (poor-mans delegation)
        // https://docs.duendesoftware.com/identityserver/v5/tokens/extension_grants/token_exchange/

        // Requirements and handlers.
        // https://app.pluralsight.com/course-player?clipId=48dfa72f-ab0c-4c06-9b84-6d92a56ee26f

        // This is an alternative registration method.
        //services.AddAuthorization(Configure);
    }

    private static void Configure(AuthorizationOptions options)
    {
        // These policy names can be assigned to the Policy property on Authorize attributes on a controller class or method.
        // Requests that do not fulfill the policy will return a 403 "forbidden" status code.
        options.AddPolicy(DefaultAuthPolicyName, policy =>
        {
            policy.RequireAuthenticatedUser();
            // Add any additional policy rules here.
        });

        // TODO: Add delegate to add custom authorization policies.
        // https://app.pluralsight.com/course-player?clipId=6f802090-fb1c-4618-b1ae-fbb54c13d789

        //options.AddDefaultPolicy();
        //options.AddDefaultFallbackPolicy();

        // https://app.pluralsight.com/course-player?clipId=7a3e77a2-2a7c-4177-825b-a51009cb36b7

        // Authorization API
        // https://app.pluralsight.com/course-player?clipId=c734bcbd-061e-493e-81bb-fe1e258ee652
        // Take a look at the token exchange (a custom grant type extension implemented in IdentityServer)
        // if a separate authorization API is used, and you don't want to reuse access tokens between APIs (poor-mans delegation)
        // https://docs.duendesoftware.com/identityserver/v5/tokens/extension_grants/token_exchange/

        // Requirements and handlers.
        // https://app.pluralsight.com/course-player?clipId=48dfa72f-ab0c-4c06-9b84-6d92a56ee26f
    }
}
