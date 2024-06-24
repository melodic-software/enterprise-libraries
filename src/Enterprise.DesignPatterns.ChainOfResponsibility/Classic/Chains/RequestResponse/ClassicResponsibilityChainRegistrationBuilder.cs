using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestResponse;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestResponse;

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

    public ClassicResponsibilityChainRegistrationBuilder<TRequest, TResponse> WithSuccessor<TChainLink>(
        ImplementationFactory<TChainLink> implementationFactory)
        where TChainLink : class, IClassicHandler<TRequest, TResponse>
    {
        _services.AddTransient<IClassicHandler<TRequest, TResponse>>(implementationFactory.Invoke);
        return this;
    }
}
