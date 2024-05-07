using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;
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
    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        Logger.LogDebug("Raising {EventCount} events.", events.Count);

        foreach (IEvent @event in events)
            await RaiseAsync(@event);

        Logger.LogDebug("{EventCount} event(s) raised.", events.Count);
    }

    public async Task RaiseAsync(IEvent @event)
    {
        using (Logger.BeginScope("Event: {EventType}", @event.GetType().Name))
        {
            await _eventDispatcher.DispatchAsync(@event);
        }
    }
}
