﻿using System.Reflection;
using Enterprise.Api.Security.Constants;
using Enterprise.Api.Swagger.Constants;
using Enterprise.Options.Core.Delegates;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Options;

/// <summary>
/// The options for Swagger configuration in the application.
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// Key for the configuration section related to Swagger settings.
    /// To preventing duplication, some of these values may be set or overridden with the "shared" config settings.
    /// </summary>
    public const string ConfigSectionKey = "Custom:Swagger";

    /// <summary>
    /// Swagger is enabled by default.
    /// It can never be enabled in a production environment (for security purposes).
    /// </summary>
    public bool EnableSwagger { get; set; }

    /// <summary>
    /// The token security service that is the centralized token authority.
    /// </summary>
    public string Authority { get; set; }

    /// <summary>
    /// The OAuth application name.
    /// </summary>
    public string OAuthAppName { get; set; }

    /// <summary>
    /// The OAuth audience.
    /// </summary>
    public string? OAuthAudience { get; set; }

    /// <summary>
    /// The OAuth client for the swagger documentation page.
    /// </summary>
    public string OAuthClientId { get; set; }

    /// <summary>
    /// The OAuth client secret.
    /// This may or not be required depending on the OAuth flow.
    /// </summary>
    public string? OAuthClientSecret { get; set; }

    /// <summary>
    /// The available OAuth scopes.
    /// </summary>
    public Dictionary<string, string> OAuthScopes { get; set; }

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
    public bool UsePkce { get; set; }

    /// <summary>
    /// The application name.
    /// </summary>
    public string ApplicationName { get; set; }

    /// <summary>
    /// A brief (optional) description of the application.
    /// </summary>
    public string ApplicationDescription { get; set; }

    /// <summary>
    /// Used to retrieve XML comments for models contained in a separate API client project/assembly.
    /// NOTE: an XML documentation file must be generated by the target project.
    /// </summary>
    public Func<Assembly>? GetApiClientAssembly { get; set; }

    /// <summary>
    /// An optional extensibility hook for adding application specific customizations.
    /// These can include operation filters, document filters, etc.
    /// </summary>
    public Configure<SwaggerGenOptions>? PostConfigure { get; set; }

    /// <summary>
    /// This allows for complete control over how swagger is configured.
    /// If provided, the prebuilt default will not be applied.
    /// </summary>
    public Configure<SwaggerGenOptions>? CustomConfigure { get; set; }

    /// <summary>
    /// Have the required OAuth configuration settings been set?
    /// </summary>
    public bool CanConfigureOAuth => !string.IsNullOrWhiteSpace(Authority) &&
                                     !string.IsNullOrWhiteSpace(OAuthClientId) &&
                                     !string.IsNullOrWhiteSpace(OAuthAppName);

    /// <summary>
    /// Create a new <see cref="SwaggerOptions"/> with the default settings.
    /// </summary>
    public SwaggerOptions()
    {
        EnableSwagger = true;
        Authority = SecurityConstants.DemoAuthority;
        OAuthAppName = SecurityConstants.DemoOAuthAppName;
        OAuthClientId = SecurityConstants.DemoOAuthClientId;
        OAuthClientSecret = null;
        OAuthScopes = new Dictionary<string, string>();
        UsePkce = true;
        ApplicationName = SwaggerConstants.DefaultAppName;
        ApplicationDescription = SwaggerConstants.DefaultAppDescription;
        GetApiClientAssembly = null;
        PostConfigure = null;
        CustomConfigure = null;
    }
}
