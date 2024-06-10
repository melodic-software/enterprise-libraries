﻿using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration.Context.Extensions;
using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Decoration;
using Enterprise.Events.Handlers.Delegates;
using Enterprise.Events.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Handlers;

public static class EventHandlerRegistrar
{
    public static void RegisterEventHandler<T>(this IServiceCollection services,
        EventHandlerImplementationFactory<T> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where T : IEvent
    {
        services.BeginRegistration<IHandleEvent<T>>()
            .Add(factory.Invoke, serviceLifetime)
            .WithDecorators((provider, eventHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IHandleEvent<T> decorator = new NullEventValidationEventHandler<T>(eventHandler, decoratorService);
                return decorator;
            }, (provider, eventHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingEventHandler<T>> logger = provider.GetRequiredService<ILogger<ErrorHandlingEventHandler<T>>>();
                IHandleEvent<T> decorator = new ErrorHandlingEventHandler<T>(eventHandler, decoratorService, logger);
                return decorator;
            }, (provider, eventHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingEventHandler<T>> logger = provider.GetRequiredService<ILogger<LoggingEventHandler<T>>>();
                IHandleEvent<T> decorator = new LoggingEventHandler<T>(eventHandler, decoratorService, logger);
                return decorator;
            });
    }
}
