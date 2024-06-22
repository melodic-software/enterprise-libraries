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
        ServiceDescriptor originalDescriptor = ServiceDescriptorService.GetDescriptor(_services, serviceType);

        // Reverse the decorators array, so they are applied in the appropriate order.
        // TODO: Should this be configurable?
        decoratorFactories = decoratorFactories.Reverse().ToArray();

        TService ImplementationFactory(IServiceProvider provider)
        {
            TService originalService = ImplementationService.GetService<TService>(originalDescriptor, provider);
            return decoratorFactories.Aggregate(originalService, (service, decoratorFactory) => decoratorFactory(provider, service));
        }

        var decoratedDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, originalDescriptor.Lifetime);

        ReplaceOriginalDescriptor(originalDescriptor, decoratedDescriptor);
    }

    private void ReplaceOriginalDescriptor(ServiceDescriptor originalDescriptor, ServiceDescriptor decoratedDescriptor)
    {
        bool originalRemoved = _services.Remove(originalDescriptor);
        _services.Add(decoratedDescriptor);
    }
}
