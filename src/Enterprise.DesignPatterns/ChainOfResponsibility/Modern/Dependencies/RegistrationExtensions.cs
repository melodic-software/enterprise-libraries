using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Dependencies;

public static class RegistrationExtensions
{
    public static ResponsibilityChainRegistrationBuilder<TRequest>
        RegisterChainOfResponsibility<TRequest>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest>);
        Type concreteType = typeof(ResponsibilityChain<TRequest>);
        var serviceDescriptor = new ServiceDescriptor(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ResponsibilityChainRegistrationBuilder<TRequest>(services);
    }

    public static ResponsibilityChainRegistrationBuilder<TRequest, TResponse>
        RegisterChainOfResponsibility<TRequest, TResponse>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest, TResponse>);
        Type concreteType = typeof(ResponsibilityChain<TRequest, TResponse>);
        var serviceDescriptor = new ServiceDescriptor(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ResponsibilityChainRegistrationBuilder<TRequest, TResponse>(services);
    }
}
