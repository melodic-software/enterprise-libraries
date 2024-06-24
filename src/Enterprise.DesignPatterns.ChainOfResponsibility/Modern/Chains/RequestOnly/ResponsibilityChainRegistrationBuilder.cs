using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;
using Enterprise.DI.Core.Registration.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains.RequestOnly;
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
