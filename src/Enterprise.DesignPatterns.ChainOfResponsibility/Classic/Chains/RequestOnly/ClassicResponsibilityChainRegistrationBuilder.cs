using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestOnly;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestOnly;

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

    public ClassicResponsibilityChainRegistrationBuilder<TRequest> WithSuccessor<TSuccessor>(
        ImplementationFactory<TSuccessor> implementationFactory)
        where TSuccessor : class, IClassicHandler<TRequest>
    {
        _services.AddTransient<IClassicHandler<TRequest>>(implementationFactory.Invoke);
        return this;
    }
}
