using Enterprise.Events.Model;

namespace Enterprise.Domain.Events.Model.Abstract;

public abstract class DomainEvent : Event, IDomainEvent
{
    protected DomainEvent(Guid id, DateTimeOffset dateOccurred) : base(id, dateOccurred)
    {

    }

    protected DomainEvent(DateTimeOffset dateOccurred) : this(Guid.NewGuid(), dateOccurred)
    {

    }

    protected DomainEvent() : this(Guid.NewGuid(), DateTimeOffset.UtcNow)
    {

    }
}