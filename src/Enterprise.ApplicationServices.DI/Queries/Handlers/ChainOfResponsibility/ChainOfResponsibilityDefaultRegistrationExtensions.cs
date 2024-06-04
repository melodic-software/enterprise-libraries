using Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;

public static class ChainOfResponsibilityDefaultRegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TQuery, TResponse>(
        this IServiceCollection services,
        HandlerImplementationFactory<TQuery, TResponse> implementationFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TQuery : IBaseQuery
    {
        services.RegisterChainOfResponsibility<TQuery, TResponse>()
            .WithSuccessor<LoggingQueryHandler<TQuery, TResponse>>()
            .WithSuccessor<ErrorHandlingQueryHandler<TQuery, TResponse>>()
            .WithSuccessor<NullQueryValidationQueryHandler<TQuery, TResponse>>()
            .WithSuccessor<FluentValidationQueryHandler<TQuery, TResponse>>()
            .WithSuccessor(implementationFactory.Invoke, serviceLifetime);
    }
}
