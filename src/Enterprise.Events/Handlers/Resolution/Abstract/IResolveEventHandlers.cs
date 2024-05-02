using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Resolution.Abstract;

public interface IResolveEventHandlers
{
    Task<IEnumerable<IHandleEvent>> ResolveAsync(IEvent @event);
    Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent;
}