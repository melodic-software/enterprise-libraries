using Enterprise.DI.Core.Registration.Decoration.Delegates;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.Core.Registration.Model;

public partial class RegistrationContext<TService> where TService : class
{
    /// <summary>
    /// Registers decorators for the service, each taking the service instance and IServiceProvider as parameters.
    /// </summary>
    public RegistrationContext<TService> WithDecorators(params DecoratorFactory<TService>[] decoratorFactories)
    {
        ApplyDecoratorFactory<TService>[] applyDecoratorFactories = decoratorFactories
            .Select(factory => new ApplyDecoratorFactory<TService>(provider => service => factory(provider, service)))
            .ToArray();

        return ApplyDecorators(applyDecoratorFactories);
    }

    /// <summary>
    /// Registers decorators for the service, each represented as a factory function.
    /// </summary>
    public RegistrationContext<TService> WithDecorators(params ApplyDecoratorFactory<TService>[] applyDecoratorFactories)
    {
        return ApplyDecorators(applyDecoratorFactories);
    }

    /// <summary>
    /// Registers a single decorator for the service.
    /// </summary>
    public RegistrationContext<TService> WithDecorator<TDecorator>(
        DecoratorFactory<TService, TDecorator> decoratorFactory)
        where TDecorator : class, TService
    {
        Type serviceType = typeof(TService);

        ServiceDescriptor? originalServiceDescriptor = _services
            .FirstOrDefault(d => d.ServiceType == serviceType);

        if (originalServiceDescriptor == null)
        {
            throw new InvalidOperationException(
                $"The service of type {serviceType.Name} has not been registered and cannot be decorated."
            );
        }

        ServiceLifetime lifetime = originalServiceDescriptor.Lifetime;

        object ImplementationFactory(IServiceProvider serviceProvider)
        {
            TService originalService = GetImplementation(originalServiceDescriptor, serviceProvider);
            return decoratorFactory(serviceProvider, originalService);
        }

        var decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);

        _services.Replace(decoratorDescriptor);

        return this;
    }

    private RegistrationContext<TService> ApplyDecorators(ApplyDecoratorFactory<TService>[] applyDecoratorFactories)
    {
        ServiceDescriptor serviceDescriptor = GetServiceDescriptor();

        ImplementationFactory<TService> implementationFactory =
            CreateDecoratedImplementationFactory(serviceDescriptor, applyDecoratorFactories);

        ReplaceServiceDescriptor(serviceDescriptor, implementationFactory);

        return this;
    }

    private static ImplementationFactory<TService> CreateDecoratedImplementationFactory(
        ServiceDescriptor serviceDescriptor,
        ApplyDecoratorFactory<TService>[] applyDecoratorFactories
    )
    {
        return provider =>
        {
            TService service = GetImplementation(serviceDescriptor, provider);

            foreach (ApplyDecoratorFactory<TService> applyDecoratorFactory in applyDecoratorFactories)
            {
                ApplyDecorator<TService> applyDecorator = applyDecoratorFactory(provider);
                service = applyDecorator(service);
            }

            return service;
        };
    }
}
