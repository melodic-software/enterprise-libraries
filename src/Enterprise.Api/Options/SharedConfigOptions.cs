namespace Enterprise.Api.Options;

public class SharedConfigOptions
{
    public const string ConfigSectionKey = "Custom:Shared";

    /// <summary>
    /// This is the name of the application.
    /// Examples: "Configuration.API", "Billing.Service", etc.
    /// This is what will appear in logs, and other non display uses.
    /// </summary>
    public string? ApplicationName { get; set; }

    /// <summary>
    /// This is the friendly display name of the application.
    /// Examples: "Configuration API", "Billing Service", etc.
    /// This will be used for Swagger configuration, etc.
    /// </summary>
    public string? ApplicationDisplayName { get; set; }

    /// <summary>
    /// The trusted OAuth authority.
    /// This will be used for API security, and Swagger configuration.
    /// </summary>
    public string? OAuthAuthority { get; set; }
}