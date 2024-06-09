using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic;

public static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand, TResult>> AddChainOfResponsibility<TCommand, TResult>(
        this RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext,
        RegistrationOptions<TCommand, TResult> options,
        IServiceCollection services)
        where TCommand : class, ICommand<TResult>
    {
        if (options.ConfigureChainOfResponsibility == null)
        {
            if (options.CommandHandlerImplementationFactory == null)
            {
                throw new InvalidOperationException(
                    "A handler factory must be configured for command handler registrations " +
                    "that use the chain of responsibility design pattern."
                );
            }

            services.RegisterDefaultChainOfResponsibility(options.CommandHandlerImplementationFactory, options.ServiceLifetime);
        }
        else
        {
            // Initialize a builder instance that can be used to customize the chain.
            ResponsibilityChainRegistrationBuilder<TCommand, TResult> chainRegistrationBuilder =
                services.RegisterChainOfResponsibility<TCommand, TResult>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        return registrationContext;
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResult>> AddCommandHandler<TCommand, TResult>(
        this RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext,
        RegistrationOptions<TCommand, TResult> options) where TCommand : class, ICommand<TResult>
    {
        // Register the primary abstraction.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleCommand<TCommand, TResult>),
                factory: ImplementationFactory<TCommand, TResult>,
                options.ServiceLifetime
            )
        );

        // Register the base abstraction (alternate).
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleCommand<TCommand>),
                factory: ImplementationFactory<TCommand, TResult>,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }

    private static CommandHandler<TCommand, TResult> ImplementationFactory<TCommand, TResult>(IServiceProvider provider)
        where TCommand : class, ICommand<TResult>
    {
        IResponsibilityChain<TCommand, TResult> responsibilityChain =
            provider.GetRequiredService<IResponsibilityChain<TCommand, TResult>>();

        return new CommandHandler<TCommand, TResult>(responsibilityChain);
    }
}
