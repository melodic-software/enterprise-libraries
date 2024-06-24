using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic;

public static class ChainOfResponsibilityDefaultRegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TCommand, TResult>(
        this IServiceCollection services,
        HandlerImplementationFactory<TCommand, TResult> implementationFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TCommand : class, ICommand<TResult>
    {
        services.RegisterChainOfResponsibility<TCommand, TResult>()
            .WithSuccessor<LoggingCommandHandler<TCommand, TResult>>()
            .WithSuccessor<ErrorHandlingCommandHandler<TCommand, TResult>>()
            .WithSuccessor<NullCommandValidationCommandHandler<TCommand, TResult>>()
            .WithSuccessor<FluentValidationCommandHandler<TCommand, TResult>>()
            .WithSuccessor(implementationFactory.Invoke, serviceLifetime);
    }
}
