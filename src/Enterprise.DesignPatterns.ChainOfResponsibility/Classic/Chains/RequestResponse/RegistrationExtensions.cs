using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestResponse;

public static class RegistrationExtensions
{
    public static ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse>
        RegisterClassicChainOfResponsibility<TRequest, TResponse>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TRequest : class
    {
        Type interfaceType = typeof(IResponsibilityChain<TRequest, TResponse>);
        Type concreteType = typeof(ClassicResponsibilityChain<TRequest, TResponse>);
        var serviceDescriptor = ServiceDescriptor.Describe(interfaceType, concreteType, lifetime);
        services.Add(serviceDescriptor);

        return new ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse>(services);
    }
}
