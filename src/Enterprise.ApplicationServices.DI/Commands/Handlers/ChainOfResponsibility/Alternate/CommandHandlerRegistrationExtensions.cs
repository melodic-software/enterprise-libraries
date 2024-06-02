using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate;

public static class CommandHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a command handler using the chain of responsibility design pattern.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="services"></param>
    /// <param name="factory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterCommandHandler<TCommand>(this IServiceCollection services,
        Func<IServiceProvider, CommandHandlerBase<TCommand>> factory,
        Action<RegistrationOptions<TCommand>>? configureOptions = null)
        where TCommand : IBaseCommand
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new RegistrationOptions<TCommand>(factory);
        configureOptions?.Invoke(options);
        RegistrationContext<IHandleCommand<TCommand>> registrationContext = services.RegisterCommandHandler(options);
        options.PostConfigure?.Invoke(services, registrationContext);
    }
}
