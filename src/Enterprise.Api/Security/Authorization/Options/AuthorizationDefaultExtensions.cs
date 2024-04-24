using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using static Enterprise.Api.Security.Constants.SecurityConstants;

namespace Enterprise.Api.Security.Authorization.Options;

public static class AuthorizationDefaultExtensions
{
    /// <summary>
    /// This is the policy applied when an [Authorize] attribute has been applied,
    /// or an [AllowAnonymous] attribute is present and no policy has been specified.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultPolicy(this AuthorizationBuilder builder)
    {
        AuthorizationPolicy defaultPolicy = CreateDefaultPolicy();
        // This is the policy that is applied when no [Authorize] attribute is applied AND no [AllowAnonymous] attribute present
        builder.SetDefaultPolicy(defaultPolicy);
    }

    /// <summary>
    /// This is the policy applied when no [Authorize] attribute is applied AND no [AllowAnonymous] attribute is present.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultFallbackPolicy(this AuthorizationBuilder builder)
    {
        AuthorizationPolicy defaultFallbackPolicy = CreateDefaultPolicy();
        // This is null by default.
        // TODO: Make this configurable, keep the default as null.
        builder.SetFallbackPolicy(defaultFallbackPolicy);
    }

    /// <summary>
    /// This is the policy applied when an [Authorize] attribute has been applied,
    /// or an [AllowAnonymous] attribute is present and no policy has been specified.
    /// </summary>
    /// <param name="options"></param>
    public static void AddDefaultPolicy(this AuthorizationOptions options)
    {
        AuthorizationPolicy defaultPolicy = CreateDefaultPolicy();
        // This is the policy that is applied when no [Authorize] attribute is applied AND no [AllowAnonymous] attribute present.
        options.DefaultPolicy = defaultPolicy;
    }

    /// <summary>
    /// This is the policy applied when no [Authorize] attribute is applied AND no [AllowAnonymous] attribute is present.
    /// </summary>
    /// <param name="options"></param>
    public static void AddDefaultFallbackPolicy(this AuthorizationOptions options)
    {
        AuthorizationPolicy defaultFallbackPolicy = CreateDefaultPolicy();
        // This is null by default.
        // TODO: Make this configurable, and keep the default as null.
        options.FallbackPolicy = defaultFallbackPolicy;
    }

    private static AuthorizationPolicy CreateDefaultPolicy()
    {
        // you can put these in the constructor of the AuthorizationPolicyBuilder
        string[] authenticationSchemes = [JwtBearerDefaults.AuthenticationScheme, BasicAuthenticationScheme];

        AuthorizationPolicy defaultPolicy = new AuthorizationPolicyBuilder() // <- multiple checks can be applied
            //.RequireClaim(JwtClaimTypes.Role, "contributor") // <- example claim check
            .RequireAuthenticatedUser()
            // ^ the [Authorize] attribute has this requirement built in, but for a global policy this must be explicitly added
            .Build();

        return defaultPolicy;
    }
}