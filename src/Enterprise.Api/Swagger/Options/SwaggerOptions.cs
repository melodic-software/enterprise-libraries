﻿using System.Reflection;
using Enterprise.Api.Swagger.Constants;
using Microsoft.Extensions.DependencyInjection;
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
    public bool EnableSwagger { get; set; } = true;

    /// <summary>
    /// The application name.
    /// </summary>
    public string ApplicationName { get; set; } = SwaggerConstants.DefaultAppName;

    /// <summary>
    /// A brief (optional) description of the application.
    /// </summary>
    public string ApplicationDescription { get; set; } = SwaggerConstants.DefaultAppDescription;

    /// <summary>
    /// Used to retrieve XML comments for models contained in a separate API client projects/assemblies.
    /// NOTE: an XML documentation file must be generated by the target project(s).
    /// </summary>
    public List<Assembly> ApiClientAssemblies { get; } = [];

    /// <summary>
    /// This allows for complete control over how swagger is configured.
    /// If provided, the prebuilt default will not be applied.
    /// </summary>
    public Action<SwaggerGenOptions>? CustomConfigure { get; set; }

    /// <summary>
    /// An optional extensibility hook for adding application specific customizations.
    /// These can include operation filters, document filters, etc.
    /// </summary>
    public Action<SwaggerGenOptions, IServiceCollection>? PostConfigure { get; set; }
}
