using System.Reflection;

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
    /// Delegate that provides the assemblies containing the MediatR handlers.
    /// These implement IRequestHandler, INotificationHandler, etc.
    /// </summary>
    public Func<Assembly[]>? GetServicesFromAssemblies { get; set; } = null;
}