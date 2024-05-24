using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;

namespace Enterprise.Events.Raising;

public class EventRaiser : IRaiseEvents
{
    private readonly IDispatchEvents _eventDispatcher;

    public EventRaiser(IDispatchEvents eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        foreach (IEvent @event in events)
        {
            await RaiseAsync(@event);
        }
    }

    public async Task RaiseAsync(IEvent @event)
    {
        await _eventDispatcher.DispatchAsync(@event);
    }
}
