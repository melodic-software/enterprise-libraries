using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic;

public static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> AddChainOfResponsibility<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext,
        RegistrationOptions<TCommand, TResponse> options,
        IServiceCollection services)
        where TCommand : ICommand<TResponse>
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
            ResponsibilityChainRegistrationBuilder<TCommand, TResponse> chainRegistrationBuilder =
                services.RegisterChainOfResponsibility<TCommand, TResponse>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        return registrationContext;
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> AddCommandHandler<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext,
        RegistrationOptions<TCommand, TResponse> options) where TCommand : ICommand<TResponse>
    {
        // Register the primary abstraction.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleCommand<TCommand, TResponse>),
                factory: ImplementationFactory<TCommand, TResponse>,
                options.ServiceLifetime
            )
        );

        // Register the base abstraction (alternate).
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleCommand<TCommand>),
                factory: ImplementationFactory<TCommand, TResponse>,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }

    private static CommandHandler<TCommand, TResponse> ImplementationFactory<TCommand, TResponse>(IServiceProvider provider)
        where TCommand : ICommand<TResponse>
    {
        IResponsibilityChain<TCommand, TResponse> responsibilityChain =
            provider.GetRequiredService<IResponsibilityChain<TCommand, TResponse>>();

        return new CommandHandler<TCommand, TResponse>(responsibilityChain);
    }
}
