using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
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
    /// <param name="factory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterCommandHandler<TCommand, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, CommandHandlerBase<TCommand, TResponse>> factory,
        Action<RegistrationOptions<TCommand, TResponse>>? configureOptions = null)
        where TCommand : ICommand<TResponse>
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new RegistrationOptions<TCommand, TResponse>(factory);
        configureOptions?.Invoke(options);

        RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext =
            services.RegisterCommandHandler(options);

        options.PostConfigure?.Invoke(services, registrationContext);
    }
}
