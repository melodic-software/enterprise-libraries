using Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;
using Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;

public static class ChainOfResponsibilityDefaultRegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TQuery, TResult>(
        this IServiceCollection services,
        HandlerImplementationFactory<TQuery, TResult> implementationFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TQuery : class, IQuery
    {
        services.RegisterChainOfResponsibility<TQuery, TResult>()
            .WithSuccessor<LoggingQueryHandler<TQuery, TResult>>()
            .WithSuccessor<ErrorHandlingQueryHandler<TQuery, TResult>>()
            .WithSuccessor<NullQueryValidationQueryHandler<TQuery, TResult>>()
            .WithSuccessor<FluentValidationQueryHandler<TQuery, TResult>>()
            .WithSuccessor(implementationFactory.Invoke, serviceLifetime);
    }
}
