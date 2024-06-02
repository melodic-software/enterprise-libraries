using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate;

public static class ChainOfResponsibilityDefaultRegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TCommand, TResponse>(
        this IServiceCollection services,
        Func<IServiceProvider, IHandler<TCommand, TResponse>> implementationFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TCommand : IBaseCommand
    {
        services.RegisterChainOfResponsibility<TCommand, TResponse>()
            .WithSuccessor<LoggingCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<ErrorHandlingCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<NullCommandValidationCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<FluentValidationCommandHandler<TCommand, TResponse>>()
            .WithSuccessor(implementationFactory, serviceLifetime);
    }
}
