using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility;

public static class RegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TCommand>(
        this IServiceCollection services,
        Func<IServiceProvider, IHandler<TCommand>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TCommand : IBaseCommand
    {
        services.RegisterChainOfResponsibility<TCommand>()
            .WithSuccessor<LoggingCommandHandler<TCommand>>()
            .WithSuccessor<ErrorHandlingCommandHandler<TCommand>>()
            .WithSuccessor<NullCommandValidationCommandHandler<TCommand>>()
            .WithSuccessor<FluentValidationCommandHandler<TCommand>>()
            .WithSuccessor(factory, serviceLifetime);
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> RegisterCommandHandler<TCommand>(
        this IServiceCollection services,
        RegistrationOptions<TCommand> options)
        where TCommand : IBaseCommand
    {
        return services
            .BeginRegistration<IHandleCommand<TCommand>>()
            .AddChainOfResponsibility(options, services);
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> AddChainOfResponsibility<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registration,
        RegistrationOptions<TCommand> options,
        IServiceCollection services)
        where TCommand : IBaseCommand
    {
        if (options.ConfigureChainOfResponsibility == null)
        {
            if (options.CommandHandlerFactory == null)
            {
                throw new InvalidOperationException(
                    "A handler factory must be configured for command handler registrations " +
                    "that use the chain of responsibility design pattern."
                );
            }

            services.RegisterDefaultChainOfResponsibility(options.CommandHandlerFactory, options.ServiceLifetime);
        }
        else
        {
            // Initialize a builder instance that can be used to customize the chain.
            ResponsibilityChainRegistrationBuilder<TCommand> chainRegistrationBuilder =
                services.RegisterChainOfResponsibility<TCommand>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        // This is a command handler implementation that takes in a responsibility chain.
        registration.Add(provider =>
        {
            IResponsibilityChain<TCommand> responsibilityChain =
                provider.GetRequiredService<IResponsibilityChain<TCommand>>();

            return new CommandHandler<TCommand>(responsibilityChain);
        }, options.ServiceLifetime);

        return registration;
    }
}
