using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestOnly;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains.RequestOnly;

public static class RegistrationExtensions
{
    public static ResponsibilityChainRegistrationBuilder<TRequest>
        RegisterChainOfResponsibility<TRequest>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest>);
        Type concreteType = typeof(ResponsibilityChain<TRequest>);
        var serviceDescriptor = ServiceDescriptor.Describe(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ResponsibilityChainRegistrationBuilder<TRequest>(services);
    }
}
