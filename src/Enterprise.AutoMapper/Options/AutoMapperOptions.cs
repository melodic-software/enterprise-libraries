using System.Reflection;
namespace Enterprise.AutoMapper.Options;

public class AutoMapperOptions
{
    public const string ConfigSectionKey = "Custom:AutoMapper";

    /// <summary>
    /// Determines if AutoMapper services will be registered.
    /// Defaults to true.
    /// </summary>
    public bool EnableAutoMapper { get; set; } = true;

    /// <summary>
    /// This is a collection of assemblies that contain mapping profiles.
    /// If there are no assemblies added to this collection, a fallback will be used that loads solution assemblies.
    /// </summary>
    public List<Assembly> Assemblies { get; } = [];
}
