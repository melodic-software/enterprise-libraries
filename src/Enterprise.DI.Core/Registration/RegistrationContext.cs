using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.Core.Registration;

/// <summary>
/// Provides a context for fluent service registration, allowing for easy addition of services and decorators.
/// </summary>
/// <typeparam name="TService">The type of service to be registered.</typeparam>
public class RegistrationContext<TService> where TService : class
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// Provides a context for fluent service registration, allowing for easy addition of services and decorators.
    /// </summary>
    public RegistrationContext(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Registers a service of the specified type with a specific implementation type and service lifetime.
    /// </summary>
    /// <typeparam name="TImplementation">The implementation type of the service to be registered.</typeparam>
    /// <param name="serviceLifetime">The lifetime of the service being registered (Singleton, Scoped, or Transient).</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> Add<TImplementation>(ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);
        Type implementationType = typeof(TImplementation);
        var serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, serviceLifetime);
        _services.Add(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service of the specified type with a specific implementation type and service lifetime if it hasn't already been registered.
    /// </summary>
    /// <typeparam name="TImplementation">The implementation type of the service to be registered.</typeparam>
    /// <param name="serviceLifetime">The lifetime of the service being registered (Singleton, Scoped, or Transient).</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> TryAdd<TImplementation>(ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);
        Type implementationType = typeof(TImplementation);
        var serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, serviceLifetime);
        _services.TryAdd(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service of the specified type with a factory method for creating the service instance and service lifetime.
    /// </summary>
    /// <param name="implementationFactory">The factory method used to create the service instance.</param>
    /// <param name="serviceLifetime">The lifetime of the service being registered (Singleton, Scoped, or Transient).</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> Add(ImplementationFactory<TService> implementationFactory, ServiceLifetime serviceLifetime)
    {
        Type serviceType = typeof(TService);
        var serviceDescriptor = new ServiceDescriptor(serviceType, implementationFactory, serviceLifetime);
        _services.Add(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service of the specified type with a factory method for creating the service instance and service lifetime if it hasn't already been registered.
    /// </summary>
    /// <param name="implementationFactory">The factory method used to create the service instance.</param>
    /// <param name="serviceLifetime">The lifetime of the service being registered (Singleton, Scoped, or Transient).</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> TryAdd(ImplementationFactory<TService> implementationFactory, ServiceLifetime serviceLifetime)
    {
        Type serviceType = typeof(TService);
        var serviceDescriptor = new ServiceDescriptor(serviceType, implementationFactory, serviceLifetime);
        _services.TryAdd(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service using a pre-configured ServiceDescriptor.
    /// </summary>
    /// <param name="serviceDescriptor">The ServiceDescriptor that encapsulates all the information to register the service.</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> Add(ServiceDescriptor serviceDescriptor)
    {
        _services.Add(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service using a pre-configured ServiceDescriptor if it hasn't already been registered.
    /// </summary>
    /// <param name="serviceDescriptor">The ServiceDescriptor that encapsulates all the information to register the service.</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> TryAdd(ServiceDescriptor serviceDescriptor)
    {
        _services.TryAdd(serviceDescriptor);
        return this;
    }

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
            TService originalService = GetOriginalService(originalServiceDescriptor, serviceProvider);
            return decoratorFactory(serviceProvider, originalService);
        }

        var decoratorDescriptor = ServiceDescriptor.Describe(serviceType, ImplementationFactory, lifetime);

        _services.Replace(decoratorDescriptor);

        return this;
    }

    public RegistrationContext<TService> RegisterAlternate<TAlternate>() where TAlternate : TService
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
            TService originalService = GetOriginalService(originalServiceDescriptor, serviceProvider);
            return (TAlternate)originalService;
        }

        Type alternateType = typeof(TAlternate);
        var alternateDescriptor = ServiceDescriptor.Describe(alternateType, ImplementationFactory, lifetime);

        _services.TryAdd(alternateDescriptor);

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
            TService service = GetOriginalService(serviceDescriptor, provider);

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

    private static TService GetOriginalService(ServiceDescriptor serviceDescriptor, IServiceProvider provider)
    {
        if (serviceDescriptor.ImplementationFactory != null)
        {
            return (TService)serviceDescriptor.ImplementationFactory(provider);
        }

        if (serviceDescriptor.ImplementationInstance != null)
        {
            return (TService)serviceDescriptor.ImplementationInstance;
        }

        if (serviceDescriptor.ImplementationType != null)
        {
            return (TService)ActivatorUtilities.CreateInstance(provider, serviceDescriptor.ImplementationType);
        }

        throw new InvalidOperationException(
            "The registration method for the original service is not supported."
        );
    }
}
