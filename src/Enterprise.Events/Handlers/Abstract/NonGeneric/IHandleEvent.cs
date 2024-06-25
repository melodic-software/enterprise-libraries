using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Abstract.NonGeneric;

public interface IHandleEvent
{
    Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default);
}
