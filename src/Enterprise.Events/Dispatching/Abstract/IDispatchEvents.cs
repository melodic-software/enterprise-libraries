using Enterprise.Events.Model;
using Enterprise.Events.Raising.Callbacks.Abstractions;

namespace Enterprise.Events.Dispatching.Abstract;

public interface IDispatchEvents
{
    public Task DispatchAsync(IEvent @event, IRaiseEventCallbacks? callbackService = null);
}