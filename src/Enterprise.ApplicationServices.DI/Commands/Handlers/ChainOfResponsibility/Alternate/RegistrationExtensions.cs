using Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate;

public static class RegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TCommand, TResponse>(
        this IServiceCollection services,
        Func<IServiceProvider, IHandler<TCommand, TResponse>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TCommand : IBaseCommand
    {
        services.RegisterChainOfResponsibility<TCommand, TResponse>()
            .WithSuccessor<LoggingCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<ErrorHandlingCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<NullCommandValidationCommandHandler<TCommand, TResponse>>()
            .WithSuccessor<FluentValidationCommandHandler<TCommand, TResponse>>()
            .WithSuccessor(factory, serviceLifetime);
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> RegisterCommandHandler<TCommand, TResponse>(
        this IServiceCollection services,
        RegistrationOptions<TCommand, TResponse> options)
        where TCommand : ICommand<TResponse>
    {
        return services
            .BeginRegistration<IHandleCommand<TCommand, TResponse>>()
            .AddChainOfResponsibility(options, services);
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> AddChainOfResponsibility<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registration,
        RegistrationOptions<TCommand, TResponse> options,
        IServiceCollection services)
        where TCommand : ICommand<TResponse>
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
            ResponsibilityChainRegistrationBuilder<TCommand, TResponse> chainRegistrationBuilder =
                services.RegisterChainOfResponsibility<TCommand, TResponse>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        // This is a command handler implementation that takes in a responsibility chain.
        registration.Add(provider =>
        {
            IResponsibilityChain<TCommand, TResponse> responsibilityChain =
                provider.GetRequiredService<IResponsibilityChain<TCommand, TResponse>>();

            return new CommandHandler<TCommand, TResponse>(responsibilityChain);
        }, options.ServiceLifetime);

        return registration;
    }
}
