using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Dispatching.Decoration;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Dispatching;

internal class EventDispatchingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.BeginRegistration<IDispatchEvents>()
            .TryAddScoped(CreateEventDispatcher)
            .WithDecorator((provider, eventDispatcher) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IRaiseEventCallbacks eventCallbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
                ILogger<CallbackRaisingEventDispatchDecorator> logger = provider.GetRequiredService<ILogger<CallbackRaisingEventDispatchDecorator>>();
                return new CallbackRaisingEventDispatchDecorator(eventDispatcher, decoratorService, eventCallbackRaiser, logger);
            });

        // This is a secondary registration that does not utilize the callback raising decorator (which requires a scoped lifetime).
        // This uses a separate, more specific interface that can be explicitly used in specific scenarios to dispatch events that have been queued up.
        services.TryAddSingleton<IDispatchQueuedEvents>(CreateEventDispatcher);
    }

    private static EventDispatcher CreateEventDispatcher(IServiceProvider provider)
    {
        // It is very likely that we will have a decorator chain with the event handler(s) that are resolved.
        // This is only used to get the innermost decorated instance of the event handler resolved internally.
        IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
        IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
        ILogger<EventDispatcher> logger = provider.GetRequiredService<ILogger<EventDispatcher>>();
        return new EventDispatcher(decoratorService, eventHandlerResolver, logger);
    }
}
