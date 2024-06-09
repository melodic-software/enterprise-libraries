using System.Reflection;
using Enterprise.Api.Minimal.Mapping;

namespace Enterprise.Api.Minimal.Options;

public class MinimalApiOptions
{
    public const string ConfigSectionKey = "Custom:MinimalApi";

    public bool EnableMinimalApiEndpoints { get; set; } = true;

    /// <summary>
    /// A collection of assemblies that contain implementations of <see cref="IMapEndpoint"/> or <see cref="IMapEndpoints"/>.
    /// If this collection is empty, a fallback will be provided that loads all solution assemblies.
    /// </summary>
    public List<Assembly> EndpointAssemblies { get; } = [];
}
