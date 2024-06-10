using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DI.Core.Registration.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility;

public static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand>> AddChainOfResponsibility<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        RegistrationOptions<TCommand> options,
        IServiceCollection services)
        where TCommand : class, ICommand
    {
        if (options.ConfigureChainOfResponsibility == null)
        {
            if (options.CommandHandlerImplementationFactory == null)
            {
                throw new InvalidOperationException(
                    "A handler implementation factory must be configured for command handler registrations " +
                    "that use the chain of responsibility design pattern."
                );
            }

            services.RegisterDefaultChainOfResponsibility(
                options.CommandHandlerImplementationFactory,
                options.ServiceLifetime
            );
        }
        else
        {
            // Initialize a builder instance that can be used to customize the chain.
            ResponsibilityChainRegistrationBuilder<TCommand> chainRegistrationBuilder =
                services.RegisterChainOfResponsibility<TCommand>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        return registrationContext;
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> AddCommandHandler<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        RegistrationOptions<TCommand> options) where TCommand : class, ICommand
    {
        registrationContext.Add(
            ServiceDescriptor.Describe(
                typeof(IHandleCommand<TCommand>),
                ImplementationFactory<TCommand>,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }

    public static IHandleCommand<TCommand> ImplementationFactory<TCommand>(IServiceProvider provider)
        where TCommand : class, ICommand
    {
        IResponsibilityChain<TCommand> responsibilityChain = provider.GetRequiredService<IResponsibilityChain<TCommand>>();

        // This is a command handler implementation that takes in a responsibility chain.
        return new CommandHandler<TCommand>(responsibilityChain);
    }
}
