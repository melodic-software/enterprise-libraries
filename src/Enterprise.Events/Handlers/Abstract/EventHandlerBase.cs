using Enterprise.Events.Model;
using static Enterprise.Events.Handlers.Validation.EventHandlerTypeValidationService;

namespace Enterprise.Events.Handlers.Abstract;

public abstract class EventHandlerBase<T> : IHandleEvent<T> where T : IEvent
{
    /// <inheritdoc />
    public Task HandleAsync(IEvent @event)
    {
        ValidateType(@event, this);

        // It's safe to cast since we've validated the type.
        T typedEvent = (T)@event;

        // Delegate to the typed HandleAsync method.
        return HandleAsync(typedEvent);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(T @event);
}
