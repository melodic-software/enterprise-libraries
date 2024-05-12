using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;
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

    public ResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>(Func<IServiceProvider, TSuccessor> factory)
        where TSuccessor : class, IHandler<TRequest>
    {
        _services.AddTransient<IHandler<TRequest>>(factory);
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

    public ResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TChainLink>(Func<IServiceProvider, TChainLink> factory)
        where TChainLink : class, IHandler<TRequest, TResponse>
    {
        _services.AddTransient<IHandler<TRequest, TResponse>>(factory);
        return this;
    }
}
