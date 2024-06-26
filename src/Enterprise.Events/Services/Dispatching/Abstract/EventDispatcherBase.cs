﻿using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Handlers;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Services.Dispatching.Abstract;

public abstract class EventDispatcherBase : IDispatchEvents
{
    protected readonly IGetDecoratedInstance DecoratorService;
    protected readonly ILogger<EventDispatcherBase> Logger;

    protected EventDispatcherBase(IGetDecoratedInstance decoratorService, ILogger<EventDispatcherBase> logger)
    {
        DecoratorService = decoratorService;
        Logger = logger;
    }

    /// <inheritdoc />
    public virtual async Task DispatchAsync(IEvent @event, IRaiseEventCallbacks? callbackService = null)
    {
        Logger.LogDebug("Resolving event handlers.");
        ICollection<IHandleEvent> eventHandlers = (await ResolveEventHandlers(@event)).ToList();
        Logger.LogDebug("Resolved {EventHandlerCount} handler(s).", eventHandlers.Count);

        Logger.LogDebug("Filtering event handlers.");
        IReadOnlyCollection<IHandleEvent> filteredHandlers = FilterHandlers(eventHandlers, @event).ToList();
        Logger.LogDebug("Event handlers filtered.");

        if (!filteredHandlers.Any())
        {
            await OnNoHandlersRegistered(@event);
            return;
        }

        Logger.LogDebug("Processing event handler(s).");

        foreach (IHandleEvent eventHandler in filteredHandlers)
            await ProcessEventHandler(eventHandler, @event);

        Logger.LogDebug("Event handlers processed.");

        if (callbackService == null)
        {
            Logger.LogWarning("No event callback service provided.");
            return;
        }

        Logger.LogDebug("Raising event callbacks.");
        callbackService.RaiseCallbacks(@event);
        Logger.LogDebug("Event callbacks completed.");
    }

    public virtual IEnumerable<IHandleEvent> FilterHandlers(ICollection<IHandleEvent> eventHandlers, IEvent @event)
    {
        return eventHandlers;
    }

    public virtual Task OnNoHandlersRegistered(IEvent @event)
    {
        Logger.LogInformation("No event handlers registered for event.");
        return Task.CompletedTask;
    }

    protected virtual async Task ProcessEventHandler(IHandleEvent eventHandler, IEvent @event)
    {
        try
        {
            // IF we have decorated the event handler, this will return the inner handler.
            IHandleEvent innerHandler = DecoratorService.GetInnermost(eventHandler);

            Type eventHandlerType = innerHandler.GetType();

            Logger.LogDebug("Executing event handler: {EventHandlerType}", eventHandlerType.Name);
            await eventHandler.HandleAsync(@event);
            Logger.LogDebug("Event handled by: {EventHandlerType}.", eventHandlerType.Name);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred during event handler execution for event.");
            throw;
        }
    }

    protected abstract Task<IEnumerable<IHandleEvent>> ResolveEventHandlers(IEvent @event);
}