using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Handlers;

public interface IResolveEventHandlers
{
    Task<IEnumerable<IHandleEvent>> ResolveAsync(IEvent @event);
    Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent;
}