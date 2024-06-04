using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility;

public static class ChainOfResponsibilityDefaultRegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TCommand>(
        this IServiceCollection services,
        HandlerImplementationFactory<TCommand> implementationFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TCommand : IBaseCommand
    {
        services.RegisterChainOfResponsibility<TCommand>()
            .WithSuccessor<LoggingCommandHandler<TCommand>>()
            .WithSuccessor<ErrorHandlingCommandHandler<TCommand>>()
            .WithSuccessor<NullCommandValidationCommandHandler<TCommand>>()
            .WithSuccessor<FluentValidationCommandHandler<TCommand>>()
            .WithSuccessor(implementationFactory.Invoke, serviceLifetime);
    }
}
