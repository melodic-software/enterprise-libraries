using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;
using Enterprise.Events.Raising.Callbacks.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising;

public class EventRaiser : IRaiseEvents
{
    private readonly IDispatchEvents _eventDispatcher;

    protected readonly ILogger<EventRaiser> Logger;

    public EventRaiser(IDispatchEvents eventDispatcher,
        ILogger<EventRaiser> logger)
    {
        _eventDispatcher = eventDispatcher;

        Logger = logger;
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events, IRaiseEventCallbacks? callbackService = null)
    {
        Logger.LogDebug("Raising {EventCount} events.", events.Count);

        foreach (var @event in events)
            await RaiseAsync(@event, callbackService);

        Logger.LogDebug("{EventCount} event(s) raised.", events.Count);
    }

    public async Task RaiseAsync(IEvent @event, IRaiseEventCallbacks? callbackService = null)
    {
        using (Logger.BeginScope("Event: {EventType}", @event.GetType().Name))
        {
            await _eventDispatcher.DispatchAsync(@event, callbackService);
        }
    }
}