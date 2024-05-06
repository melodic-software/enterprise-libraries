﻿using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Decoration.Abstract;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Decorators.EventHandlers;

public class LoggingEventHandler<T> : EventHandlerDecoratorBase<T> where T : IEvent
{
    private readonly ILogger<LoggingEventHandler<T>> _logger;

    public LoggingEventHandler(IHandleEvent<T> eventHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingEventHandler<T>> logger)
        : base(eventHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(T @event)
    {
        Type eventType = typeof(T);
        Type eventHandlerType = Innermost.GetType();

        // TODO: Do we want to add a scope (or log statement) that describes the decorator chain?
        // Maybe we do that in the base?

        using (_logger.BeginScope("Event Handler: {EventHandlerType}, Event: {EventType}", eventHandlerType.Name, eventType.Name))
        {
            _logger.LogDebug("Handling event.");
            await Decorated.HandleAsync(@event);
            _logger.LogDebug("Event was handled successfully.");
        }
    }
}