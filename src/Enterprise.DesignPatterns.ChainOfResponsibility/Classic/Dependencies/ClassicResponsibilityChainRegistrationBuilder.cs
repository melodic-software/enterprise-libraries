using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Dependencies;

public class ClassicResponsibilityChainRegistrationBuilder<TRequest> where TRequest : class
{
    private readonly IServiceCollection _services;

    public ClassicResponsibilityChainRegistrationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ClassicResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>()
        where TSuccessor : class, IClassicHandler<TRequest>
    {
        _services.AddTransient<IClassicHandler<TRequest>, TSuccessor>();
        return this;
    }

    public ClassicResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>(Func<IServiceProvider, TSuccessor> factory)
        where TSuccessor : class, IClassicHandler<TRequest>
    {
        _services.AddTransient<IClassicHandler<TRequest>>(factory);
        return this;
    }
}

public class ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse> where TRequest : class
{
    private readonly IServiceCollection _services;

    public ClassicResponsibilityChainRegistrationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TChainLink>()
        where TChainLink : class, IClassicHandler<TRequest, TResponse>
    {
        _services.AddTransient<IClassicHandler<TRequest, TResponse>, TChainLink>();
        return this;
    }

    public ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TChainLink>(Func<IServiceProvider, TChainLink> factory)
        where TChainLink : class, IClassicHandler<TRequest, TResponse>
    {
        _services.AddTransient<IClassicHandler<TRequest, TResponse>>(factory);
        return this;
    }
}
