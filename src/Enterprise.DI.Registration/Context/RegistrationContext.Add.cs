using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.Registration.Context;

public partial class RegistrationContext<TService> where TService : class
{
    /// <summary>
    /// Registers a service of the specified type with a specific implementation type and service lifetime.
    /// </summary>
    /// <typeparam name="TImplementation">The implementation type of the service to be registered.</typeparam>
    /// <param name="serviceLifetime">The lifetime of the service being registered.</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> Add<TImplementation>(ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);
        Type implementationType = typeof(TImplementation);
        var serviceDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, serviceLifetime);
        _services.Add(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service of the specified type with a specific implementation type and service lifetime if it hasn't already been registered.
    /// </summary>
    /// <typeparam name="TImplementation">The implementation type of the service to be registered.</typeparam>
    /// <param name="serviceLifetime">The lifetime of the service being registered.</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> TryAdd<TImplementation>(ServiceLifetime serviceLifetime)
        where TImplementation : class, TService
    {
        Type serviceType = typeof(TService);
        Type implementationType = typeof(TImplementation);
        var serviceDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, serviceLifetime);
        _services.TryAdd(serviceDescriptor);
        return this;
    }

    /// <summary>
    /// Registers a service of the specified type with a factory method for creating the service instance and lifetime.
    /// </summary>
    /// <param name="implementationFactory">The factory method used to create the service instance.</param>
    /// <param name="serviceLifetime">The lifetime of the service being registered.</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> Add(ImplementationFactory<TService> implementationFactory, ServiceLifetime serviceLifetime)
    {
        var serviceDescriptor = ServiceDescriptor.Describe(
            typeof(TService),
            implementationFactory.Invoke,
            serviceLifetime
        );

        _services.Add(serviceDescriptor);

        return this;
    }

    /// <summary>
    /// Registers a service of the specified type, and service lifetime using a factory method for creating the service instance.
    /// The service descriptor will be added if one does not already exist for the service type.
    /// </summary>
    /// <param name="implementationFactory">The factory method used to create the service instance.</param>
    /// <param name="serviceLifetime">The lifetime of the service being registered.</param>
    /// <returns>The updated RegistrationContext instance for fluent chaining.</returns>
    public RegistrationContext<TService> TryAdd(ImplementationFactory<TService> implementationFactory, ServiceLifetime serviceLifetime)
    {
        var serviceDescriptor = ServiceDescriptor.Describe(
            typeof(TService),
            implementationFactory.Invoke,
            serviceLifetime
        );

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
}
