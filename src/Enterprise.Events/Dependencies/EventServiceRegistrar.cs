using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Dispatching;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Dispatching.Decoration;
using Enterprise.Events.Handlers.Decoration;
using Enterprise.Events.Handlers.Resolution;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Raising;
using Enterprise.Events.Raising.Abstract;
using Enterprise.Events.Raising.Callbacks;
using Enterprise.Events.Raising.Callbacks.Abstractions;
using Enterprise.Events.Raising.Callbacks.Decorators;
using Enterprise.Events.Raising.Callbacks.Facade;
using Enterprise.Events.Raising.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Dependencies;

internal class EventServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Add configuration options to allow for dynamic registration of services, decorators, etc.

        RegisterEventCallbackServices(services);

        services.BeginRegistration<IResolveEventHandlers>()
            .AddSingleton(provider => new ReflectionEventHandlerResolver(provider))
            .WithDecorators((provider, eventHandlerResolver) =>
            {
                ILogger<CachingEventHandlerResolver> logger = provider.GetRequiredService<ILogger<CachingEventHandlerResolver>>();
                return new CachingEventHandlerResolver(eventHandlerResolver, logger);
            }, (provider, eventHandlerResolver) =>
            {
                ILogger<LoggingEventHandlerResolver> logger = provider.GetRequiredService<ILogger<LoggingEventHandlerResolver>>();
                return new LoggingEventHandlerResolver(eventHandlerResolver, logger);
            });

        // We only want to add this if no other services have been registered.
        services.BeginRegistration<IDispatchEvents>()
            .TryAddSingleton(provider =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
                ILogger<EventDispatcher> logger = provider.GetRequiredService<ILogger<EventDispatcher>>();
                return new EventDispatcher(decoratorService, eventHandlerResolver, logger);
            }).WithDecorator((provider, eventDispatcher) =>
            {
                IRaiseEventCallbacks eventCallbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
                ILogger<EventCallbackRaisingDecorator> logger = provider.GetRequiredService<ILogger<EventCallbackRaisingDecorator>>();
                return new EventCallbackRaisingDecorator(eventDispatcher, eventCallbackRaiser, logger);
            });

        services.AddSingleton<IRaiseEvents>(provider =>
        {
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<EventRaiser> logger = provider.GetRequiredService<ILogger<EventRaiser>>();
            return new EventRaiser(eventDispatcher, logger);
        });
    }

    private static void RegisterEventCallbackServices(IServiceCollection services)
    {
        services.BeginRegistration<IEventCallbackRegistrar>()
            .Add(provider =>
            {
                ILogger<EventCallbackRegistrar> logger = provider.GetRequiredService<ILogger<EventCallbackRegistrar>>();
                IEventCallbackRegistrar eventCallbackRegistrar = new EventCallbackRegistrar(logger);
                return eventCallbackRegistrar;
            }, ServiceLifetime.Scoped)
            .WithDecorator((provider, eventCallbackRegistrar) =>
            {
                ILogger<LoggingEventCallbackRegistrar> logger = provider.GetRequiredService<ILogger<LoggingEventCallbackRegistrar>>();
                IEventCallbackRegistrar decorator = new LoggingEventCallbackRegistrar(eventCallbackRegistrar, logger);
                return decorator;
            });

        services.BeginRegistration<IRaiseEventCallbacks>()
            .Add(provider =>
            {
                IEventCallbackRegistrar callbackRegistrar = provider.GetRequiredService<IEventCallbackRegistrar>();
                ILogger<EventCallbackRaiser> logger = provider.GetRequiredService<ILogger<EventCallbackRaiser>>();
                IRaiseEventCallbacks callbackRaiser = new EventCallbackRaiser(callbackRegistrar, logger);
                return callbackRaiser;
            }, ServiceLifetime.Scoped)
            .WithDecorator((provider, eventCallbackRaiser) =>
            {
                ILogger<LoggingEventCallbackRaiser> logger = provider.GetRequiredService<ILogger<LoggingEventCallbackRaiser>>();
                IRaiseEventCallbacks decorator = new LoggingEventCallbackRaiser(eventCallbackRaiser, logger);
                return decorator;
            });

        services.AddScoped<IEventCallbackService>(provider =>
        {
            IEventCallbackRegistrar callbackRegistrar = provider.GetRequiredService<IEventCallbackRegistrar>();
            IRaiseEventCallbacks callbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
            return new EventCallbackService(callbackRegistrar, callbackRaiser);
        });
    }
}
