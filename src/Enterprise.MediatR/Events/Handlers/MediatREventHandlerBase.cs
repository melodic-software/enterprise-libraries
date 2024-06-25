using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Model;
using MediatR;

namespace Enterprise.MediatR.Events.Handlers;

public abstract class MediatREventHandlerBase<T> : EventHandlerBase<T>, INotificationHandler<T>
    where T : IEvent, INotification
{
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        await HandleAsync(notification, cancellationToken);
    }
}
