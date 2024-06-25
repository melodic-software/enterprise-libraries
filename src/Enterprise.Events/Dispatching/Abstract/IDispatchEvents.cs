using Enterprise.Events.Model;

namespace Enterprise.Events.Dispatching.Abstract;

public interface IDispatchEvents
{
    public Task DispatchAsync(IEvent[] events, CancellationToken cancellationToken = default);
    public Task DispatchAsync(IEvent @event, CancellationToken cancellationToken = default);
}
