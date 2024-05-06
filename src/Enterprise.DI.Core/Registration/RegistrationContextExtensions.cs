using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.Registration;

public static class RegistrationContextExtensions
{
    /// <summary>
    /// Registers a singleton service of the specified implementation type.
    /// </summary>
    public static RegistrationContext<TService> AddSingleton<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService  =>
        registrationContext.Add<TImplementation>(ServiceLifetime.Singleton);

    /// <summary>
    /// Registers a singleton service with a factory for creating the service.
    /// </summary>
    public static RegistrationContext<TService> AddSingleton<TService>(
        this RegistrationContext<TService> registrationContext, Func<IServiceProvider, TService> implementationFactory)
        where TService : class =>
        registrationContext.Add(implementationFactory, ServiceLifetime.Singleton);

    /// <summary>
    /// Registers a singleton service of the specified implementation type if it hasn't already been registered.
    /// </summary>
    public static RegistrationContext<TService> TryAddSingleton<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.TryAdd<TImplementation>(ServiceLifetime.Singleton);

    /// <summary>
    /// Registers a singleton service with a factory for creating the service if it hasn't already been registered.
    /// </summary>
    public static RegistrationContext<TService> TryAddSingleton<TService>(
        this RegistrationContext<TService> registrationContext, Func<IServiceProvider, TService> implementationFactory)
        where TService : class =>
        registrationContext.TryAdd(implementationFactory, ServiceLifetime.Singleton);

    /// <summary>
    /// Registers a scoped service of the specified implementation type.
    /// </summary>
    public static RegistrationContext<TService> AddScoped<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.Add<TImplementation>(ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a scoped service with a factory for creating the service.
    /// </summary>
    public static RegistrationContext<TService> AddScoped<TService>(
        this RegistrationContext<TService> registrationContext, Func<IServiceProvider, TService> implementationFactory)
        where TService : class =>
        registrationContext.Add(implementationFactory, ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a scoped service of the specified implementation type if it hasn't already been registered.
    /// </summary>
    public static RegistrationContext<TService> TryAddScoped<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.TryAdd<TImplementation>(ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a scoped service with a factory for creating the service if it hasn't already been registered.
    /// </summary>
    public static RegistrationContext<TService> TryAddScoped<TService>(
        this RegistrationContext<TService> registrationContext, Func<IServiceProvider, TService> implementationFactory)
        where TService : class =>
        registrationContext.TryAdd(implementationFactory, ServiceLifetime.Scoped);

    /// <summary>
    /// Registers a transient service of the specified implementation type.
    /// </summary>
    public static RegistrationContext<TService> AddTransient<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.Add<TImplementation>(ServiceLifetime.Transient);

    /// <summary>
    /// Registers a transient service with a factory for creating the service.
    /// </summary>
    public static RegistrationContext<TService> AddTransient<TService>(
        this RegistrationContext<TService> registrationContext, Func<IServiceProvider, TService> implementationFactory)
        where TService : class =>
        registrationContext.Add(implementationFactory, ServiceLifetime.Transient);

    /// <summary>
    /// Registers a transient service of the specified implementation type if it hasn't already been registered.
    /// </summary>
    public static RegistrationContext<TService> TryAddTransient<TService, TImplementation>(
        this RegistrationContext<TService> registrationContext)
        where TService : class
        where TImplementation : class, TService =>
        registrationContext.TryAdd<TImplementation>(ServiceLifetime.Transient);

    /// <summary>
    /// Registers a transient service with a factory for creating the service if it hasn't already been registered.
    /// </summary>
    public static RegistrationContext<TService> TryAddTransient<TService>(
        this RegistrationContext<TService> registrationContext, Func<IServiceProvider, TService> implementationFactory)
        where TService : class =>
        registrationContext.TryAdd(implementationFactory, ServiceLifetime.Transient);
}
