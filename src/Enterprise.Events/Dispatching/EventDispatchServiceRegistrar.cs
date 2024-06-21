using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.DI.Registration.Context.Extensions;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Dispatching.Decoration;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Dispatching;

internal sealed class EventDispatchServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.BeginRegistration<IDispatchEvents>()
            .TryAddScoped(provider =>
            {
                // It is very likely that we will have a decorator chain with the event handler(s) that are resolved.
                // This is only used to get the innermost decorated instance of the event handler resolved internally.
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
                ILogger<EventDispatcher> logger = provider.GetRequiredService<ILogger<EventDispatcher>>();
                return new EventDispatcher(decoratorService, eventHandlerResolver, logger);
            })
            .WithDecorator((provider, eventDispatcher) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IRaiseEventCallbacks eventCallbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
                return new CallbackRaisingEventDispatchDecorator(eventDispatcher, decoratorService, eventCallbackRaiser);
            });
    }
}
