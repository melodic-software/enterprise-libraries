using Enterprise.Messages.Core.Model;
using MediatR;

namespace Enterprise.Events.Model;

public interface IEvent : IMessage, INotification
{
    /// <summary>
    /// The unique identifier for the event.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The date and time the event occurred.
    /// </summary>
    public DateTimeOffset DateOccurred { get; }
}