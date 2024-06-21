using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates;
using Enterprise.DI.Registration.Context;
using Enterprise.DI.Registration.Context.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility;

public static class CommandHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a command handler using the chain of responsibility design pattern.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterCommandHandler<TCommand>(this IServiceCollection services,
        CommandHandlerImplementationFactory<TCommand> implementationFactory,
        ConfigureOptions<TCommand>? configureOptions = null)
        where TCommand : class, ICommand
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        var options = new RegistrationOptions<TCommand>(implementationFactory.Invoke);
        configureOptions?.Invoke(options);
        RegistrationContext<IHandleCommand<TCommand>> registrationContext = services.RegisterCommandHandler(options);
        options.PostConfigure?.Invoke(services, registrationContext);
    }

    private static RegistrationContext<IHandleCommand<TCommand>> RegisterCommandHandler<TCommand>(
        this IServiceCollection services,
        RegistrationOptions<TCommand> options)
        where TCommand : class, ICommand
    {
        return services
            .BeginRegistration<IHandleCommand<TCommand>>()
            .AddChainOfResponsibility(options, services)
            .AddCommandHandler(options);
    }
}
