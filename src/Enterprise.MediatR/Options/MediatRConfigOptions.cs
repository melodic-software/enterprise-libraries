using System.Reflection;
using Enterprise.MediatR.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.MediatR.Options;

public class MediatRConfigOptions
{
    public const string ConfigSectionKey = "Custom:MediatR";

    /// <summary>
    /// Determines if MediatR services will be registered.
    /// Defaults to true.
    /// </summary>
    public bool EnableMediatR { get; set; } = true;

    /// <summary>
    /// This is a collection of assemblies that contain MediatR handlers.
    /// These implement IRequestHandler, INotificationHandler, etc.
    /// If there are no assemblies added to this collection, a fallback will be used that loads solution assemblies.
    /// </summary>
    public List<Assembly> Assemblies { get; } = [];

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
}
