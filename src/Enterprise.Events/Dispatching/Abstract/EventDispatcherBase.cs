using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Handlers.Abstract.NonGeneric;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Dispatching.Abstract;

/// <summary>
/// Base class for event dispatchers that provides common functionalities to dispatch events.
/// This class handles the resolution, filtering, and processing of event handlers based on the incoming events.
/// </summary>
public abstract class EventDispatcherBase : IDispatchEvents
{
    protected readonly IGetDecoratedInstance DecoratorService;
    protected readonly ILogger<EventDispatcherBase> Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatcherBase"/> class.
    /// </summary>
    /// <param name="decoratorService">Service to access decorated instances of handlers.</param>
    /// <param name="logger">Logger for logging operations within the dispatcher.</param>
    protected EventDispatcherBase(IGetDecoratedInstance decoratorService, ILogger<EventDispatcherBase> logger)
    {
        DecoratorService = decoratorService;
        Logger = logger;
    }

    /// <inheritdoc />
    public async Task DispatchAsync(IEvent[] events, CancellationToken cancellationToken = default)
    {
        foreach (IEvent @event in events)
        {
            await DispatchAsync(@event, cancellationToken);
        }
    }

    /// <inheritdoc />
    public virtual async Task DispatchAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Resolving event handlers.");
        ICollection<IHandleEvent> eventHandlers = (await ResolveEventHandlers(@event)).ToList();
        Logger.LogDebug("Resolved {EventHandlerCount} handler(s).", eventHandlers.Count);

        if (eventHandlers.Count <= 0)
        {
            await OnNoHandlersRegistered(@event);
            return;
        }

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
        {
            await ProcessEventHandlerAsync(eventHandler, @event, cancellationToken);
        }

        Logger.LogDebug("Event handlers processed.");
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

    protected virtual async Task ProcessEventHandlerAsync(IHandleEvent eventHandler, IEvent @event, CancellationToken cancellationToken = default)
    {
        try
        {
            // IF we have decorated the event handler, this will return the inner handler.
            IHandleEvent innerHandler = DecoratorService.GetInnermost(eventHandler);

            Type eventHandlerType = innerHandler.GetType();

            Logger.LogDebug("Executing event handler: {EventHandlerType}", eventHandlerType.Name);
            await eventHandler.HandleAsync(@event, cancellationToken);
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
