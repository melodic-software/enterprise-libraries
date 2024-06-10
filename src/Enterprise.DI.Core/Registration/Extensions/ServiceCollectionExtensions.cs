using Enterprise.DI.Core.Registration.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.Registration.Extensions;

public static class ServiceCollectionExtensions
{
    public static RegistrationContext<TService> BeginRegistration<TService>(this IServiceCollection services)
        where TService : class
    {
        return new RegistrationContext<TService>(services);
    }

    /// <summary>
    /// Registers an open generic type with the dependency injection (DI) container.
    /// </summary>
    /// <typeparam name="T">The generic interface or class type to be registered.</typeparam>
    /// <typeparam name="T2">The concrete type that implements or derives from <typeparamref name="T"/>.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <param name="serviceLifetime">Specifies the lifetime of the service in the DI container.</param>
    /// <exception cref="ArgumentException">Thrown if either <typeparamref name="T"/> or <typeparamref name="T2"/> are not open generic types.</exception>
    public static void RegisterOpenGeneric<T, T2>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where T : class // Enforces that T is an interface or a class.
        where T2 : class, T // Enforces that T2 implements or derives from T.
    {
        if (!typeof(T).IsGenericTypeDefinition || !typeof(T2).IsGenericTypeDefinition)
        {
            throw new ArgumentException("Both T and T2 must be open generic types.");
        }

        var serviceDescriptor = new ServiceDescriptor(typeof(T), typeof(T2), serviceLifetime);

        services.Add(serviceDescriptor);
    }
}
