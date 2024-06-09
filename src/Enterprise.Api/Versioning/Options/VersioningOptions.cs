using Asp.Versioning;

namespace Enterprise.Api.Versioning.Options;

public class VersioningOptions
{
    public const string ConfigSectionKey = "Custom:Versioning";

    /// <summary>
    /// This is the default version of the API when one is not specified.
    /// Ensure this default is properly configured when older versions become deprecated or removed.
    /// </summary>
    public ApiVersion DefaultApiVersion { get; set; } = ApiVersion.Default;

    /// <summary>
    /// Enables support for versioning via a url segment (like Google and Facebook) -> "api/v1/resource".
    /// Defaults to true.
    /// </summary>
    public bool EnableUrlVersioning { get; set; } = true;

    /// <summary>
    /// Enables specifying the version via a query string parameter.
    /// This is the out-of-the-box default .NET strategy.
    /// Defaults to true.
    /// </summary>
    public bool EnableQueryStringVersioning { get; set; } = true;

    /// <summary>
    /// This uses a custom HTTP header.
    /// Defaults to true.
    /// </summary>
    public bool EnableHeaderVersioning { get; set; } = true;

    /// <summary>
    /// Defaults to "application/json;v=2.0" but can be specified in the constructor (something like "version" or "v").
    /// Defaults to true.
    /// </summary>
    public bool EnableMediaTypeVersioning { get; set; } = true;
}
