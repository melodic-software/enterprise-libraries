using Enterprise.DI.Registration.Context.Delegates;
using Enterprise.DI.Registration.Context.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Registration.Context;

public partial class RegistrationContext<TService> where TService : class
{
    /// <summary>
    /// Registers decorators for the service, each taking the service instance and IServiceProvider as parameters.
    /// </summary>
    public RegistrationContext<TService> WithDecorators(params DecoratorFactory<TService>[] decoratorFactories)
    {
        ArgumentNullException.ThrowIfNull(decoratorFactories);
        RegisterDecorators(decoratorFactories);
        return this;
    }

    /// <summary>
    /// Registers a single decorator for the service.
    /// </summary>
    public RegistrationContext<TService> WithDecorator(DecoratorFactory<TService> decoratorFactory)
    {
        ArgumentNullException.ThrowIfNull(decoratorFactory);
        RegisterDecorators([decoratorFactory]);
        return this;
    }

    private void RegisterDecorators(DecoratorFactory<TService>[] decoratorFactories)
    {
        Type serviceType = typeof(TService);
        ServiceDescriptor originalDescriptor = GetOriginalServiceDescriptor(serviceType);

        Func<IServiceProvider, TService> originalFactory = ImplementationService.GetImplementationFactory<TService>(originalDescriptor);

        // Reverse the decorators array, so they are applied in the appropriate order.
        DecoratorFactory<TService>[] reversedDecorators = decoratorFactories.Reverse().ToArray();

        TService ImplementationFactory(IServiceProvider provider)
        {
            TService originalService = originalFactory(provider);
            return reversedDecorators.Aggregate(originalService, (service, factory) => factory(provider, service));
        }

        // Register as a new descriptor to avoid overwriting the original
        var decoratedDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, originalDescriptor.Lifetime);

        ReplaceOriginalDescriptor(originalDescriptor, decoratedDescriptor);
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

    private void ReplaceOriginalDescriptor(ServiceDescriptor originalDescriptor, ServiceDescriptor decoratedDescriptor)
    {
        int index = _services.IndexOf(originalDescriptor);

        if (index != -1)
        {
            _services[index] = decoratedDescriptor;
        }
        else
        {
            _services.Add(decoratedDescriptor);
        }
    }
}
