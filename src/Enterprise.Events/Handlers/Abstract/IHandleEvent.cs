using Enterprise.Events.Handlers.Abstract.NonGeneric;
using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Abstract;

public interface IHandleEvent<in T> : IHandleEvent where T : IEvent
{
    Task HandleAsync(T @event);
}
