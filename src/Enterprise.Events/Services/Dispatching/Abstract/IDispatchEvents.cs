using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;

namespace Enterprise.Events.Services.Dispatching.Abstract;

public interface IDispatchEvents
{
    public Task DispatchAsync(IEvent @event, IRaiseEventCallbacks? callbackService = null);
}