using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic;

public static class ChainOfResponsibilityDefaultRegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TCommand, TResponse>(
        this IServiceCollection services,
        HandlerImplementationFactory<TCommand, TResponse> implementationFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TCommand : ICommand<TResponse>
    {
        services.RegisterChainOfResponsibility<TCommand, TResponse>()
            .WithSuccessor<LoggingCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<ErrorHandlingCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<NullCommandValidationCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<FluentValidationCommandHandler<TCommand, TResponse>>()
            .WithSuccessor(implementationFactory.Invoke, serviceLifetime);
    }
}
