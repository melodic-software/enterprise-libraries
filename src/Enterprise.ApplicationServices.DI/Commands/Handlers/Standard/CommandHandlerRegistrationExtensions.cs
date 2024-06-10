using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Shared;
using Enterprise.DI.Core.Registration.Extensions;
using Enterprise.DI.Core.Registration.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard;

public static class CommandHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a command handler.
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
        var options = new RegistrationOptions<TCommand>(implementationFactory);
        configureOptions?.Invoke(options);
        RegistrationContext<IHandleCommand<TCommand>> registrationContext = services.RegisterCommandHandler(options);
        options.PostConfigure?.Invoke(services, registrationContext);
    }

    private static RegistrationContext<IHandleCommand<TCommand>> RegisterCommandHandler<TCommand>(
        this IServiceCollection services,
        RegistrationOptions<TCommand> options)
        where TCommand : class, ICommand
    {
        RegistrationContext<IHandleCommand<TCommand>> registrationContext = services
            .BeginRegistration<IHandleCommand<TCommand>>();

        if (options.UseDecorators)
        {
            return registrationContext.RegisterWithDecorators(options);
        }

        return registrationContext.AddCommandHandler(options);
    }
}
