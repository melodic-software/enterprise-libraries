using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Registration.Context.Extensions;

public static partial class RegistrationContextExtensions
{
    /// <summary>
    /// Registers a scoped service of the specified implementation type.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="registrationContext"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> AddScoped<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.Add<TImplementation>(ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a keyed scoped service of the specified implementation type.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="registrationContext"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> AddKeyedScoped<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext, object? serviceKey)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.AddKeyed<TImplementation>(serviceKey, ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a scoped service with a factory for creating the service.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="registrationContext"></param>
    /// <param name="implementationFactory"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> AddScoped<TService>(
        this RegistrationContext<TService> registrationContext, ImplementationFactory<TService> implementationFactory)
        where TService : class =>
        registrationContext.Add(implementationFactory, ServiceLifetime.Scoped);


    /// <summary>
    /// Registers a keyed scoped service with a factory for creating the service.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="registrationContext"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> AddScopedKeyed<TService>(
        this RegistrationContext<TService> registrationContext, 
        KeyedImplementationFactory<TService> implementationFactory,
        object? serviceKey)
        where TService : class =>
        registrationContext.AddKeyed(implementationFactory, serviceKey, ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a scoped service of the specified implementation type (if it hasn't already been registered).
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="registrationContext"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> TryAddScoped<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.TryAdd<TImplementation>(ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a keyed scoped service of the specified implementation type (if it hasn't already been registered).
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="registrationContext"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> TryAddKeyedScoped<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext, object? serviceKey)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.TryAddKeyed<TImplementation>(serviceKey, ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a scoped service with a factory for creating the service (if it hasn't already been registered).
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="registrationContext"></param>
    /// <param name="implementationFactory"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> TryAddScoped<TService>(
        this RegistrationContext<TService> registrationContext, ImplementationFactory<TService> implementationFactory)
        where TService : class =>
        registrationContext.TryAdd(implementationFactory, ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a keyed scoped service with a factory for creating the service (if it hasn't already been registered).
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="registrationContext"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    public static RegistrationContext<TService> TryAddKeyedScoped<TService>(
        this RegistrationContext<TService> registrationContext, 
        KeyedImplementationFactory<TService> implementationFactory,
        object? serviceKey)
        where TService : class =>
        registrationContext.TryAddKeyed(implementationFactory, serviceKey, ServiceLifetime.Scoped);
}
