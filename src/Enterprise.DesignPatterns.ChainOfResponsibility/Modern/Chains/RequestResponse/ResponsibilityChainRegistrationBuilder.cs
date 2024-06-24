using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestResponse;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains.RequestResponse;

public class ResponsibilityChainRegistrationBuilder<TRequest, TResponse> where TRequest : class
{
    private readonly IServiceCollection _services;

    public ResponsibilityChainRegistrationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TSuccessor>()
        where TSuccessor : class, IHandler<TRequest, TResponse>
    {
        _services.AddTransient<IHandler<TRequest, TResponse>, TSuccessor>();
        return this;
    }

    public ResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TSuccessor>(
        ImplementationFactory<TSuccessor> implementationFactory)
        where TSuccessor : class, IHandler<TRequest, TResponse>
    {
        _services.AddTransient<IHandler<TRequest, TResponse>>(implementationFactory.Invoke);
        return this;
    }
}
