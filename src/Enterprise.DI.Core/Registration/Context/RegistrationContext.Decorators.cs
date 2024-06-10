using Enterprise.DI.Core.Registration.Context.Delegates;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.DI.Core.Registration.Context.Services;

namespace Enterprise.DI.Core.Registration.Context;

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
            TService originalService = ImplementationService.Get<TService>(originalServiceDescriptor, serviceProvider);
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

    private ServiceDescriptor GetServiceDescriptor()
    {
        ServiceDescriptor? serviceDescriptor = _services
            .FirstOrDefault(d => d.ServiceType == typeof(TService));

        if (serviceDescriptor == null)
        {
            throw new InvalidOperationException(
                $"The service of type {typeof(TService).Name} has not been registered."
            );
        }

        return serviceDescriptor;
    }

    private static ImplementationFactory<TService> CreateDecoratedImplementationFactory(
        ServiceDescriptor serviceDescriptor,
        ApplyDecoratorFactory<TService>[] applyDecoratorFactories
    )
    {
        return provider =>
        {
            TService service = ImplementationService.Get<TService>(serviceDescriptor, provider);

            foreach (ApplyDecoratorFactory<TService> applyDecoratorFactory in applyDecoratorFactories)
            {
                ApplyDecorator<TService> applyDecorator = applyDecoratorFactory(provider);
                service = applyDecorator(service);
            }

            return service;
        };
    }

    private void ReplaceServiceDescriptor(ServiceDescriptor originalDescriptor,
        ImplementationFactory<TService> implementationFactory)
    {
        var serviceDescriptor = ServiceDescriptor.Describe(
            typeof(TService),
            implementationFactory.Invoke,
            originalDescriptor.Lifetime
        );

        _services.Replace(serviceDescriptor);
    }
}
