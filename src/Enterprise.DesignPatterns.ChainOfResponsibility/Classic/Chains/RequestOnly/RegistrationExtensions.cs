using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestOnly;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestOnly;

public static class RegistrationExtensions
{
    public static ClassicResponsibilityChainRegistrationBuilder<TRequest>
        RegisterClassicChainOfResponsibility<TRequest>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest>);
        Type concreteType = typeof(ClassicResponsibilityChain<TRequest>);
        var serviceDescriptor = ServiceDescriptor.Describe(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ClassicResponsibilityChainRegistrationBuilder<TRequest>(services);
    }
}
