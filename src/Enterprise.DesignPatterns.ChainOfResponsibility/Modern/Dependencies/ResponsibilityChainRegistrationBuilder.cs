using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Dependencies;

public class ResponsibilityChainRegistrationBuilder<TRequest> where TRequest : class
{
    private readonly IServiceCollection _services;

    public ResponsibilityChainRegistrationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>()
        where TSuccessor : class, IHandler<TRequest>
    {
        _services.AddTransient<IHandler<TRequest>, TSuccessor>();
        return this;
    }

    public ResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>(
        ImplementationFactory<TSuccessor> implementationFactory)
        where TSuccessor : class, IHandler<TRequest>
    {
        _services.AddTransient<IHandler<TRequest>>(implementationFactory.Invoke);
        return this;
    }
}

public class ResponsibilityChainRegistrationBuilder<TRequest, TResponse> where TRequest : class
{
    private readonly IServiceCollection _services;

    public ResponsibilityChainRegistrationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TChainLink>()
        where TChainLink : class, IHandler<TRequest, TResponse>
    {
        _services.AddTransient<IHandler<TRequest, TResponse>, TChainLink>();
        return this;
    }

    public ResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TChainLink>(
        ImplementationFactory<TChainLink> implementationFactory)
        where TChainLink : class, IHandler<TRequest, TResponse>
    {
        _services.AddTransient<IHandler<TRequest, TResponse>>(implementationFactory.Invoke);
        return this;
    }
}
