namespace Enterprise.DomainDrivenDesign.Entities.Aggregates.NonGeneric;

public class AggregateRoot : AggregateRoot<Guid>
{
    protected AggregateRoot(Guid id) : base(id)
    {
    }
}
