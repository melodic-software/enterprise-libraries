using Enterprise.Applications.DI.ServiceCollection.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DI.ServiceCollection;

/// <summary>
/// A container to hold the registered services in the application.
/// This container is intended to be registered as a singleton right before the application is built.
/// When the application is built, the service collection is locked and transformed into a <see cref="IServiceProvider"/>.
/// At this point, any new services cannot be added to the <see cref="IServiceCollection"/>.
/// </summary>
public class ServiceDescriptorRegistry : IServiceDescriptorRegistry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceDescriptorRegistry"/> class.
    /// This constructor captures the state of the provided <see cref="IServiceCollection"/>
    /// as a read-only collection.
    /// </summary>
    /// <param name="services">The service collection to be held in the container.</param>
    public ServiceDescriptorRegistry(IServiceCollection services)
    {
        // Capture the services as a read-only collection.
        ServiceDescriptors = services.ToList().AsReadOnly();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<ServiceDescriptor> ServiceDescriptors { get; }
}
