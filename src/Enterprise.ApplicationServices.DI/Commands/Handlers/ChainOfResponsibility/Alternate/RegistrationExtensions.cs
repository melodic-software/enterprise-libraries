using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate;

public static class RegistrationExtensions
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
        Func<IServiceProvider, IHandleCommand<TCommand, TResponse>> implementationFactory = provider =>
        {
            IResponsibilityChain<TCommand, TResponse> responsibilityChain =
                provider.GetRequiredService<IResponsibilityChain<TCommand, TResponse>>();

            return new CommandHandler<TCommand, TResponse>(responsibilityChain);
        };

        // This is a command handler implementation that takes in a responsibility chain.
        registrationContext.Add(implementationFactory, options.ServiceLifetime);

        // We also need to register this as a standard command handler.
        var standardImplementationFactory = implementationFactory as Func<IServiceProvider, IHandleCommand<TCommand>>;

        var serviceDescriptor = new ServiceDescriptor(
            typeof(IHandleCommand<TCommand>),
            standardImplementationFactory,
            options.ServiceLifetime
        );

        registrationContext.Add(serviceDescriptor);

        return registrationContext;
    }
}
