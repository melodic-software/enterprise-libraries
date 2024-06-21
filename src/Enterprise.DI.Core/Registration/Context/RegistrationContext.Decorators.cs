using Enterprise.DI.Core.Registration.Context.Delegates;
using Enterprise.DI.Core.Registration.Context.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.Core.Registration.Context;

public partial class RegistrationContext<TService> where TService : class
{
    /// <summary>
    /// Registers decorators for the service, each taking the service instance and IServiceProvider as parameters.
    /// </summary>
    public RegistrationContext<TService> WithDecorators(params DecoratorFactory<TService>[] decoratorFactories)
    {
        Type serviceType = typeof(TService);
        ServiceDescriptor originalServiceDescriptor = GetOriginalServiceDescriptor(serviceType);
        ServiceLifetime lifetime = originalServiceDescriptor.Lifetime;

        object ImplementationFactory(IServiceProvider serviceProvider)
        {
            TService originalService = ImplementationService.Get<TService>(originalServiceDescriptor, serviceProvider);

            TService decoratedService = originalService;

            // Ensure these are applied in reverse order.
            foreach (DecoratorFactory<TService> decoratorFactory in decoratorFactories.Reverse())
            {
                decoratedService = decoratorFactory(serviceProvider, originalService);
            }

            return decoratedService;
        }

        var decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);

        _services.Replace(decoratorDescriptor);

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
            TService originalService = ImplementationService.Get<TService>(originalServiceDescriptor, serviceProvider);
            return decoratorFactory(serviceProvider, originalService);
        }

        var decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);

        _services.Replace(decoratorDescriptor);

        return this;
    }

    private ServiceDescriptor GetOriginalServiceDescriptor(Type serviceType)
    {
        ServiceDescriptor? originalServiceDescriptor = _services
            .FirstOrDefault(d => d.ServiceType == serviceType);

        if (originalServiceDescriptor == null)
        {
            throw new InvalidOperationException(
                $"The service of type {serviceType.Name} has not been registered and cannot be decorated."
            );
        }

        return originalServiceDescriptor;
    }
}
