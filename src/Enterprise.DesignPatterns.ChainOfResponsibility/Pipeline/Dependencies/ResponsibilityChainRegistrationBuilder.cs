using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;

/// <summary>
/// Builder class for registering handlers in the chain of responsibility.
/// </summary>
/// <typeparam name="TRequest">The type of request the handlers will process.</typeparam>
public class ResponsibilityChainRegistrationBuilder<TRequest>
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// Initializes a new instance of the ChainOfResponsibilityRegistrationBuilder class.
    /// </summary>
    /// <param name="services">The service collection to which the handlers will be registered.</param>
    public ResponsibilityChainRegistrationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Registers a handler as a successor in the responsibility chain.
    /// </summary>
    /// <typeparam name="TSuccessor">The type of the handler to be registered.</typeparam>
    /// <param name="serviceLifetime">The service lifetime for the registered handler.</param>
    /// <returns>The builder instance for chaining further configuration.</returns>
    public ResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>(
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TSuccessor : class, IHandler<TRequest>
    {
        Type serviceType = typeof(IHandler<TRequest>);
        Type implementationType = typeof(TSuccessor);

        var serviceDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, serviceLifetime);

        _services.Add(serviceDescriptor);

        return this;
    }

    /// <summary>
    /// Registers a handler using a factory method as a successor in the responsibility chain.
    /// </summary>
    /// <typeparam name="TSuccessor">The type of the handler to be registered.</typeparam>
    /// <param name="implementationFactory">The factory method used to create the handler/successor.</param>
    /// <param name="serviceLifetime">The service lifetime for the registered handler.</param>
    /// <returns>The builder instance for chaining further configuration.</returns>
    public ResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>(
        ImplementationFactory<TSuccessor> implementationFactory, 
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TSuccessor : class, IHandler<TRequest>
    {
        var serviceDescriptor = ServiceDescriptor.Describe(
            typeof(IHandler<TRequest>),
            implementationFactory.Invoke,
            serviceLifetime
        );

        _services.Add(serviceDescriptor);

        return this;
    }
}


/// <summary>
/// Builder class for registering handlers in the chain of responsibility.
/// </summary>
/// <typeparam name="TRequest">The type of request the handlers will process.</typeparam>
/// <typeparam name="TResponse">The type of response the handlers will return.</typeparam>
public class ResponsibilityChainRegistrationBuilder<TRequest, TResponse>
{
    private readonly IServiceCollection _services;

    /// <summary>
    /// Initializes a new instance of the ChainOfResponsibilityRegistrationBuilder class.
    /// </summary>
    /// <param name="services">The service collection to which the handlers will be registered.</param>
    public ResponsibilityChainRegistrationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Registers a handler as a successor in the responsibility chain.
    /// </summary>
    /// <typeparam name="TSuccessor">The type of the handler to be registered.</typeparam>
    /// <param name="serviceLifetime">The service lifetime for the registered handler.</param>
    /// <returns>The builder instance for chaining further configuration.</returns>
    public ResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TSuccessor>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TSuccessor : class, IHandler<TRequest, TResponse>
    {
        Type serviceType = typeof(IHandler<TRequest, TResponse>);
        Type implementationType = typeof(TSuccessor);

        var serviceDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, serviceLifetime);

        _services.Add(serviceDescriptor);
        
        return this;
    }

    /// <summary>
    /// Registers a handler using a factory method as a successor in the responsibility chain.
    /// </summary>
    /// <typeparam name="TSuccessor">The type of the handler to be registered.</typeparam>
    /// <param name="implementationFactory">The factory method used to create the handler/successor.</param>
    /// <param name="serviceLifetime">The service lifetime for the registered handler.</param>
    /// <returns>The builder instance for chaining further configuration.</returns>
    public ResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TSuccessor>(
        ImplementationFactory<TSuccessor> implementationFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TSuccessor : class, IHandler<TRequest, TResponse>
    {
        var serviceDescriptor = ServiceDescriptor.Describe(
            typeof(IHandler<TRequest, TResponse>),
            implementationFactory.Invoke,
            serviceLifetime
        );

        _services.Add(serviceDescriptor);

        return this;
    }
}
