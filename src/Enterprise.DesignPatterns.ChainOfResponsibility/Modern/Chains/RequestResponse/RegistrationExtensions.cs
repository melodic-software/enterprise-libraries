using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains.RequestResponse;

public static class RegistrationExtensions
{
    public static ResponsibilityChainRegistrationBuilder<TRequest, TResponse>
        RegisterChainOfResponsibility<TRequest, TResponse>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest, TResponse>);
        Type concreteType = typeof(ResponsibilityChain<TRequest, TResponse>);
        var serviceDescriptor = ServiceDescriptor.Describe(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ResponsibilityChainRegistrationBuilder<TRequest, TResponse>(services);
    }
}
