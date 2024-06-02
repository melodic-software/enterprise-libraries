using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.ServiceCollection.Abstract;

/// <summary>
/// A read-only container of service descriptors for services that have been registered in the application.
/// </summary>
public interface IServiceDescriptorRegistry
{
    /// <summary>
    /// Gets the read-only collection of registered service descriptors.
    /// </summary>
    IReadOnlyCollection<ServiceDescriptor> ServiceDescriptors { get; }
}
