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
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterCommandHandler<TCommand, TResponse>(this IServiceCollection services,
        CommandHandlerImplementationFactory<TCommand, TResponse> implementationFactory,
        ConfigureOptions<TCommand, TResponse>? configureOptions = null)
        where TCommand : ICommand<TResponse>
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        var options = new RegistrationOptions<TCommand, TResponse>(implementationFactory);
        configureOptions?.Invoke(options);

        RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext =
            services.RegisterCommandHandler(options);

        options.PostConfigure?.Invoke(services, registrationContext);
    }

    private static RegistrationContext<IHandleCommand<TCommand, TResponse>> RegisterCommandHandler<TCommand, TResponse>(
        this IServiceCollection services,
        RegistrationOptions<TCommand, TResponse> options)
        where TCommand : ICommand<TResponse>
    {
        RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext = services
            .BeginRegistration<IHandleCommand<TCommand, TResponse>>();

        if (options.UseDecorators)
        {
            return registrationContext.RegisterWithDecorators(options);
        }

        return registrationContext.AddCommandHandler(options);
    }
}
