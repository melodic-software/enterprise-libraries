using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate;

public static class CommandHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a command handler using the chain of responsibility design pattern.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterCommandHandler<TCommand, TResponse>(this IServiceCollection services,
        CommandHandlerImplementationFactory<TCommand, TResponse> implementationFactory,
        Action<RegistrationOptions<TCommand, TResponse>>? configureOptions = null)
        where TCommand : ICommand<TResponse>
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        var options = new RegistrationOptions<TCommand, TResponse>(implementationFactory.Invoke);
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
        return services
            .BeginRegistration<IHandleCommand<TCommand, TResponse>>()
            .AddChainOfResponsibility(options, services)
            .AddCommandHandler(options);
    }
}
