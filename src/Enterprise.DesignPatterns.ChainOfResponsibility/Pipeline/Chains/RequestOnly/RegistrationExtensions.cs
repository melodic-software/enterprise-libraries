﻿using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestOnly;

/// <summary>
/// Contains extension methods for IServiceCollection to facilitate the registration of responsibility chains.
/// </summary>
public static class RegistrationExtensions
{
    /// <summary>
    /// Registers a responsibility chain for handling requests and responses of specified types.
    /// </summary>
    /// <typeparam name="TRequest">The type of request the chain will handle.</typeparam>
    /// <param name="services">The service collection to which the chain will be added.</param>
    /// <param name="lifetime">The service lifetime of the responsibility chain.</param>
    /// <returns>A builder for further configuration of the responsibility chain.</returns>
    public static ResponsibilityChainRegistrationBuilder<TRequest>
        RegisterChainOfResponsibility<TRequest>(this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest>);
        Type concreteType = typeof(ResponsibilityChain<TRequest>);

        var serviceDescriptor = ServiceDescriptor.Describe(interfaceType, concreteType, lifetime);

        services.Add(serviceDescriptor);

        return new ResponsibilityChainRegistrationBuilder<TRequest>(services);
    }
}