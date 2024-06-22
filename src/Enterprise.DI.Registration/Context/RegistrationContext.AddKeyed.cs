using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.Registration.Context;

public partial class RegistrationContext<TService> where TService : class
{
    /// <summary>
    /// Registers a service of the specified type with a specific implementation type, key, and service lifetime.
    /// </summary>
    /// <typeparam name="TImplementation">The implementation type of the service to be registered.</typeparam>
    /// <param name="serviceKey">The key used to register the service.</param>
    /// <param name="serviceLifetime">The lifetime of the service being registered.</param>
    /// <returns></returns>
    public RegistrationContext<TService> AddKeyed<TImplementation>(object? serviceKey, ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);
        Type implementationType = typeof(TImplementation);
        var serviceDescriptor = ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationType, serviceLifetime!);
        _services.Add(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service of the specified type with a specific implementation type, key, and service lifetime if it hasn't already been registered.
    /// </summary>
    /// <typeparam name="TImplementation">The implementation type of the service to be registered.</typeparam>
    /// <param name="serviceKey">The key used to register the service.</param>
    /// <param name="serviceLifetime">The lifetime of the service being registered.</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> TryAddKeyed<TImplementation>(object? serviceKey, ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);
        Type implementationType = typeof(TImplementation);
        var serviceDescriptor = ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationType, serviceLifetime!);
        _services.TryAdd(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service of the specified type with a factory method for creating the service instance, key, and lifetime.
    /// </summary>
    /// <param name="implementationFactory">A factory delegate that returns the implementation.</param>
    /// <param name="serviceKey">The key used to register the service.</param>
    /// <param name="serviceLifetime">The lifetime of the service being registered.</param>
    /// <returns></returns>
    public RegistrationContext<TService> AddKeyed(KeyedImplementationFactory<TService> implementationFactory, object? serviceKey, ServiceLifetime serviceLifetime)
    {
        var serviceDescriptor = ServiceDescriptor.DescribeKeyed(
            typeof(TService),
            serviceKey,
            implementationFactory.Invoke,
            serviceLifetime
        );

        _services.Add(serviceDescriptor);

        return this;
    }

    /// <summary>
    /// Registers a service of the specified type, key, and service lifetime using a factory method for creating the service instance.
    /// The service descriptor will be added if one does not already exist for the service type and key.
    /// </summary>
    /// <param name="implementationFactory">The factory method used to create the service instance.</param>
    /// <param name="serviceKey">The lifetime of the service being registered.</param>
    /// <param name="serviceLifetime">The updated RegistrationContext instance for fluent chaining.</param>
    /// <returns></returns>
    public RegistrationContext<TService> TryAddKeyed(KeyedImplementationFactory<TService> implementationFactory, object? serviceKey, ServiceLifetime serviceLifetime)
    {
        var serviceDescriptor = ServiceDescriptor.DescribeKeyed(
            typeof(TService),
            serviceKey,
            implementationFactory.Invoke,
            serviceLifetime
        );

        _services.TryAdd(serviceDescriptor);

        return this;
    }
}
