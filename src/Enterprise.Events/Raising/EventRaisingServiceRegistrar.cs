using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Dispatching;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Dispatching.Decoration;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Raising.Abstract;
using Enterprise.Events.Raising.Callbacks.Raising.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising;

internal class EventRaisingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // We only want to add this if no other services have been registered.
        services.BeginRegistration<IDispatchEvents>()
            .TryAddSingleton(provider =>
            {
                // It is very likely that we will have a decorator chain with the event handlers.
                // This is only used to get the innermost decorated instance of the event handler resolved internally.
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
                ILogger<EventDispatcher> logger = provider.GetRequiredService<ILogger<EventDispatcher>>();
                return new EventDispatcher(decoratorService, eventHandlerResolver, logger);
            }).WithDecorator((provider, eventDispatcher) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IRaiseEventCallbacks eventCallbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
                ILogger<CallbackRaisingEventDispatchDecorator> logger = provider.GetRequiredService<ILogger<CallbackRaisingEventDispatchDecorator>>();
                return new CallbackRaisingEventDispatchDecorator(eventDispatcher, decoratorService, eventCallbackRaiser, logger);
            });

        services.AddSingleton<IRaiseEvents>(provider =>
        {
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<EventRaiser> logger = provider.GetRequiredService<ILogger<EventRaiser>>();
            return new EventRaiser(eventDispatcher, logger);
        });
    }
}
