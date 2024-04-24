using System.Reflection;
namespace Enterprise.AutoMapper.Options;

public class AutoMapperConfigOptions
{
    public const string ConfigSectionKey = "Custom:AutoMapper";

    /// <summary>
    /// Determines if AutoMapper services will be registered.
    /// Defaults to true.
    /// </summary>
    public bool EnableAutoMapper { get; set; } = true;

    /// <summary>
    /// Delegate that provides the assemblies containing the AutoMapper profiles.
    /// </summary>
    public Func<Assembly[]>? GetMappingProfileAssemblies { get; set; } = null;
}