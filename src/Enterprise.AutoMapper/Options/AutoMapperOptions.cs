using System.Reflection;
using AutoMapper;
using Enterprise.AutoMapper.Assemblies;

namespace Enterprise.AutoMapper.Options;

public class AutoMapperOptions
{
    public const string ConfigSectionKey = "Custom:AutoMapper";

    /// <summary>
    /// Determines if AutoMapper services will be registered.
    /// Defaults to true.
    /// </summary>
    public bool EnableAutoMapper { get; set; } = true;

    public Action<IMapperConfigurationExpression> Configure { get; set; } = _ => { };

    /// <summary>
    /// Explicitly add an assembly containing AutoMapper profiles.
    /// This will be included when AutoMapper services are registered.
    /// </summary>
    /// <param name="assembly"></param>
    public void AddAssembly(Assembly assembly) => AutoMapperAssemblyService.Instance.AddAssembly(assembly);
}
