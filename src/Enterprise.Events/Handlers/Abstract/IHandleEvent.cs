using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Abstract;

public interface IHandleEvent
{
    Task HandleAsync(IEvent @event);
}

public interface IHandleEvent<in T> : IHandleEvent where T : IEvent
{
    Task HandleAsync(T @event);
}