using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates.Pragmatic;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Pragmatic;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Pragmatic.Delegates;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Pragmatic;

public static class CommandHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a command handler.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterCommandHandler<TCommand, TResult>(this IServiceCollection services,
        CommandHandlerImplementationFactory<TCommand, TResult> implementationFactory,
        ConfigureOptions<TCommand, TResult>? configureOptions = null)
        where TCommand : ICommand<TResult>
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        var options = new RegistrationOptions<TCommand, TResult>(implementationFactory);
        configureOptions?.Invoke(options);

        RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext =
            services.RegisterCommandHandler(options);

        options.PostConfigure?.Invoke(services, registrationContext);
    }

    private static RegistrationContext<IHandleCommand<TCommand, TResult>> RegisterCommandHandler<TCommand, TResult>(
        this IServiceCollection services,
        RegistrationOptions<TCommand, TResult> options)
        where TCommand : ICommand<TResult>
    {
        RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext = services
            .BeginRegistration<IHandleCommand<TCommand, TResult>>();

        if (options.UseDecorators)
        {
            return registrationContext.RegisterWithDecorators(options);
        }

        return registrationContext.AddCommandHandler(options);
    }
}
