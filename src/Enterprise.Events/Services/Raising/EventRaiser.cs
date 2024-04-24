using Enterprise.Events.Model;
using Enterprise.Events.Services.Dispatching.Abstract;
using Enterprise.Events.Services.Raising.Abstract;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Services.Raising;

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

        foreach (IEvent @event in events)
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