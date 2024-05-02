﻿using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Model;
using MediatR;

namespace Enterprise.MediatR.EventHandlers;

public abstract class MediatREventHandlerBase<T> : EventHandlerBase<T>, INotificationHandler<T>
    where T : IEvent, INotification
{
    public async Task Handle(T @event, CancellationToken cancellationToken)
    {
        await HandleAsync(@event);
    }
}
