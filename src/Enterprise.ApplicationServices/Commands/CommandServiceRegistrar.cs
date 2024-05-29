using Enterprise.ApplicationServices.Commands.Dispatching;
using Enterprise.ApplicationServices.Commands.Facade;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Commands;

internal sealed class CommandServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            IResolveCommandHandler commandHandlerResolver = new CommandHandlerResolver(provider);

            return commandHandlerResolver;
        });

        services.AddScoped(provider =>
        {
            IResolveCommandHandler commandHandlerResolver = provider.GetRequiredService<IResolveCommandHandler>();
            IDispatchCommands commandDispatcher = new CommandDispatcher(commandHandlerResolver);
            return commandDispatcher;
        });

        services.AddScoped(provider =>
        {
            IDispatchCommands commandDispatcher = provider.GetRequiredService<IDispatchCommands>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
            ICommandDispatchFacade commandDispatchFacade = new CommandDispatchFacade(commandDispatcher, eventCallbackService);
            return commandDispatchFacade;
        });

        // These are implementations of the null object pattern.
        // They log warnings internally, but this ensures the dispatching services do not have to check for nulls.
        // Registering these makes it easier to resolve them from the DI container.
        services.AddTransient(typeof(NullCommandHandler<>));
        services.AddTransient(typeof(NullCommandHandler<,>));
    }
}
