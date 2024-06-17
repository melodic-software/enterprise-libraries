namespace Enterprise.Api.Swagger.Options;

public class SwaggerUIOptions
{
    /// <summary>
    /// Key for the configuration section related to Swagger UI settings.
    /// </summary>
    public const string ConfigSectionKey = "Custom:Swagger:UI";

    /// <summary>
    /// This allows for complete control over how the swagger UI is configured.
    /// If provided, the prebuilt default will not be applied.
    /// </summary>
    public Action<Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions>? CustomConfigureUI { get; set; }

    /// <summary>
    /// An optional extensibility hook for adding application specific UI customizations.
    /// </summary>
    public Action<Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions>? PostConfigureUI { get; set; }

}
