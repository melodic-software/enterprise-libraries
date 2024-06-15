using System.Collections.Concurrent;
using System.Reflection;
using Enterprise.MediatR.Assemblies;
using Enterprise.MediatR.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.MediatR.Options;

public class MediatROptions
{
    public const string ConfigSectionKey = "Custom:MediatR";

    /// <summary>
    /// Determines if MediatR services will be registered.
    /// Defaults to true.
    /// </summary>
    public bool EnableMediatR { get; set; } = true;

    /// <summary>
    /// Allows for providing a custom behavior pipeline.
    /// This will override any default behavior, so be sure to include all behaviors needed for the application in a specific order.
    /// </summary>
    public List<BehaviorRegistration> BehaviorRegistrations { get; } = [];

    /// <summary>
    /// This allows for complete customization and control over the MediatR config.
    /// None of the default configuration is used, so everything must be completely configured.
    /// </summary>
    public Action<MediatRServiceConfiguration>? CustomConfigure { get; set; }

    /// <summary>
    /// Explicitly add an assembly containing MediatR types.
    /// This will be registered in the MediatR configuration delegates if MediatR has been enabled.
    /// </summary>
    /// <param name="assembly"></param>
    public void AddAssembly(Assembly assembly) => MediatRAssemblyService.Instance.AddAssembly(assembly);
}
