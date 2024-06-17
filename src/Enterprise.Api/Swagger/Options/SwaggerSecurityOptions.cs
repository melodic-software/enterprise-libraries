using Enterprise.Api.Security.Constants;

namespace Enterprise.Api.Swagger.Options;

public class SwaggerSecurityOptions
{
    /// <summary>
    /// Key for the configuration section related to Swagger security settings.
    /// To preventing duplication, some of these values may be set or overridden with the "shared" config settings.
    /// </summary>
    public const string ConfigSectionKey = "Custom:Swagger:Security";

    /// <summary>
    /// The token security service that is the centralized token authority.
    /// </summary>
    public string Authority { get; set; } = SecurityConstants.DemoAuthority;

    /// <summary>
    /// The OAuth application name.
    /// </summary>
    public string OAuthAppName { get; set; } = SecurityConstants.DemoOAuthAppName;

    /// <summary>
    /// The OAuth audience.
    /// </summary>
    public string? OAuthAudience { get; set; }

    /// <summary>
    /// The OAuth client for the swagger documentation page.
    /// </summary>
    public string OAuthClientId { get; set; } = SecurityConstants.DemoOAuthClientId;

    /// <summary>
    /// The OAuth client secret.
    /// This may or not be required depending on the OAuth flow.
    /// </summary>
    public string? OAuthClientSecret { get; set; }

    /// <summary>
    /// The available OAuth scopes.
    /// </summary>
    public Dictionary<string, string> OAuthScopes { get; set; } = new();

    /// <summary>
    /// Determines if the OpenAPiOAuthFlow.AuthorizationCode flow is enabled.
    /// </summary>
    public bool EnableAuthorizationCodeFlow { get; set; } = true;

    /// <summary>
    /// Determines if the OpenAPiOAuthFlow.Implicit flow is enabled.
    /// </summary>
    public bool EnableImplicitFlow { get; set; }

    /// <summary>
    /// Determines if the OpenAPiOAuthFlow.ClientCredentials flow is enabled.
    /// </summary>
    public bool EnableClientCredentialsFlow { get; set; }

    /// <summary>
    /// Determines if the OpenAPiOAuthFlow.Password flow is enabled.
    /// </summary>
    public bool EnablePasswordFlow { get; set; }

    /// <summary>
    /// Use Proof Key for Code Exchange.
    /// This only applies to authorization code flows.
    /// </summary>
    public bool UsePkce { get; set; } = true;

    /// <summary>
    /// Have the required OAuth configuration settings been set?
    /// </summary>
    public bool CanConfigureOAuth => !string.IsNullOrWhiteSpace(Authority) &&
                                     !string.IsNullOrWhiteSpace(OAuthClientId) &&
                                     !string.IsNullOrWhiteSpace(OAuthAppName);
}
