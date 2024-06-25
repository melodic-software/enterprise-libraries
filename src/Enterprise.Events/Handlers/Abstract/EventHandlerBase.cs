﻿using Enterprise.Events.Model;
using static Enterprise.Events.Handlers.Validation.EventHandlerTypeValidationService;

namespace Enterprise.Events.Handlers.Abstract;

public abstract class EventHandlerBase<T> : IHandleEvent<T> where T : IEvent
{
    /// <inheritdoc />
    public Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        ValidateType(@event, this);

        // It's safe to cast since we've validated the type.
        var typedEvent = (T)@event;

        // Delegate to the typed HandleAsync method.
        return HandleAsync(typedEvent, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(T @event, CancellationToken cancellationToken = default);
}
