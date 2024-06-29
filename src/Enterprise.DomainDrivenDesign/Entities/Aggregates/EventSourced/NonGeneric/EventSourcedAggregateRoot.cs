namespace Enterprise.DomainDrivenDesign.Entities.Aggregates.EventSourced.NonGeneric;

public abstract class EventSourcedAggregateRoot : EventSourcedAggregateRoot<Guid>
{
    protected EventSourcedAggregateRoot(Guid id) : base(id)
    {
    }
}
