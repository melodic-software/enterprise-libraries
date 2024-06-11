using Enterprise.Api.Security.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Enterprise.Api.Security.Options;

public class JwtBearerTokenOptions
{
    public const string ConfigSectionKey = "Custom:JwtBearerToken";

    /// <summary>
    /// The URL of the identity provider/security token service that issues trusted tokens.
    /// </summary>
    public string Authority { get; set; } = SecurityConstants.DemoAuthority;

    /// <summary>
    /// The intended audience for the token.
    /// This is a claim that specifies the name of the application or system the token is meant for.
    /// NOTE: This claim was created by Microsoft and is not part of the OAuth2 standard.
    /// </summary>
    public string Audience { get; set; } = SecurityConstants.DemoAudience;

    /// <summary>
    /// The claim type used for the name of the authenticated user.
    /// </summary>
    public string? NameClaimType { get; set; } = SecurityConstants.DefaultJwtNameClaimType;

    /// <summary>
    /// The set of valid token types for token validation.
    /// By default, only the "at+jwt" format is supported, ensuring that only JWT tokens are accepted to prevent "JWT" confusion attacks.
    /// </summary>
    public HashSet<string> ValidTokenTypes { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        SecurityConstants.AccessTokenInJwtFormatType
    };

    /// <summary>
    /// The set of valid issuers that can issue tokens.
    /// This can be useful for scenarios with multiple URIs or port mappings.
    /// If empty, the <see cref="Authority"/> value will be used as the issuer.
    /// An issuer is identified by the "iss" claim in the JWT.
    /// </summary>
    public HashSet<string> ValidIssuers { get; } = [];

    /// <summary>
    /// Specifies whether HTTPS metadata is required.
    /// By default, HTTPS is only required in the production environment.
    /// This value can be explicitly set to override the default behavior.
    /// </summary>
    public bool? RequireHttpsMetadata { get; set; }

    /// <summary>
    /// A custom delegate to completely customize the <see cref="JwtBearerOptions"/>.
    /// NOTE: If this is set, all default configurations are ignored.
    /// </summary>
    public Action<JwtBearerOptions>? ConfigureJwtBearerOptions { get; set; }
}
