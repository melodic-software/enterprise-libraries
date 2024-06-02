using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Alternate;

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
        Func<IServiceProvider, CommandHandlerBase<TCommand, TResponse>> implementationFactory,
        Action<RegistrationOptions<TCommand, TResponse>>? configureOptions = null)
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
            registrationContext.RegisterWithDecorators(options);
        }
        else
        {
            registrationContext.AddCommandHandler(options);
        }

        return registrationContext;
    }
}
