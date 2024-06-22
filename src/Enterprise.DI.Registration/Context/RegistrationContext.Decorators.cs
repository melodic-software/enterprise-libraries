using Enterprise.DI.Registration.Context.Delegates;
using Enterprise.DI.Registration.Context.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.Registration.Context;

public partial class RegistrationContext<TService> where TService : class
{
    /// <summary>
    /// Registers decorators for the service, each taking the service instance and IServiceProvider as parameters.
    /// </summary>
    public RegistrationContext<TService> WithDecorators(params DecoratorFactory<TService>[] decoratorFactories)
    {
        Type serviceType = typeof(TService);

        // Retrieve the original service descriptor based on the service type.
        ServiceDescriptor originalServiceDescriptor = GetOriginalServiceDescriptor(serviceType);
        ServiceLifetime lifetime = originalServiceDescriptor.Lifetime;

        // Implementation factory that creates the decorated service.
        object ImplementationFactory(IServiceProvider serviceProvider)
        {
            // Resolve the original service instance.
            var originalService = (TService)serviceProvider.GetRequiredService(serviceType);

            // Start with the original service.
            TService decoratedService = originalService;

            // Apply decorators in reverse order for correct decorator chaining.
            foreach (DecoratorFactory<TService> decoratorFactory in decoratorFactories.Reverse())
            {
                decoratedService = decoratorFactory(serviceProvider, decoratedService);
            }

            return decoratedService;
        }

        // Create a new descriptor for the decorated service with the same lifetime as the original.
        var decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);

        // Replace the original service descriptor with the new decorated one.
        _services.Replace(decoratorDescriptor);

        // Return the registration context for chaining.
        return this;
    }
    
    /// <summary>
    /// Registers a single decorator for the service.
    /// </summary>
    public RegistrationContext<TService> WithDecorator(DecoratorFactory<TService> decoratorFactory)
    {
        Type serviceType = typeof(TService);

        ServiceDescriptor originalServiceDescriptor = GetOriginalServiceDescriptor(serviceType);

        ServiceLifetime lifetime = originalServiceDescriptor.Lifetime;

        object ImplementationFactory(IServiceProvider serviceProvider)
        {
            TService originalService = ImplementationService.GetService<TService>(originalServiceDescriptor, serviceProvider);
            return decoratorFactory(serviceProvider, originalService);
        }

        var decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);

        _services.Replace(decoratorDescriptor);

        return this;
    }

    private ServiceDescriptor GetOriginalServiceDescriptor(Type serviceType)
    {
        ServiceDescriptor? originalServiceDescriptor = _services
            .LastOrDefault(d => d.ServiceType == serviceType);

        if (originalServiceDescriptor == null)
        {
            throw new InvalidOperationException(
                $"The service of type {serviceType.Name} has not been registered and cannot be decorated."
            );
        }

        return originalServiceDescriptor;
    }
}
