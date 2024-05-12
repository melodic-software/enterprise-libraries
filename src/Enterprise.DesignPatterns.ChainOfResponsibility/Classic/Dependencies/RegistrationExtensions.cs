using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Dependencies;

public static class RegistrationExtensions
{
    public static ClassicResponsibilityChainRegistrationBuilder<TRequest>
        RegisterClassicChainOfResponsibility<TRequest>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest>);
        Type concreteType = typeof(ClassicResponsibilityChain<TRequest>);
        var serviceDescriptor = new ServiceDescriptor(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ClassicResponsibilityChainRegistrationBuilder<TRequest>(services);
    }

    public static ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse>
        RegisterClassicChainOfResponsibility<TRequest, TResponse>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest, TResponse>);
        Type concreteType = typeof(ClassicResponsibilityChain<TRequest, TResponse>);
        var serviceDescriptor = new ServiceDescriptor(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse>(services);
    }
}
