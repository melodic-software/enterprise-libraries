using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.DomainDrivenDesign.Entities.Aggregates;

/// <summary>
/// An aggregate root is an entity that is composed of one or more entities, value objects, etc.
/// This base implementation includes basic functionalities for domain event recording and management.
/// This class should be inherited by any aggregate root in the domain model to ensure proper event handling and state management.
/// </summary>
/// <typeparam name="TId">The type of the identifier for the aggregate root (entity).</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : IEquatable<TId>
{
    /// <summary>
    /// This is the collection of internal domain events.
    /// These are changes that have been made and recorded within the aggregate root.
    /// </summary>
    protected readonly List<IDomainEvent> DomainEvents = [];

    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Record a domain event that has occurred within the aggregate boundary.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected virtual void Record(IDomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => DomainEvents.AsReadOnly();

    public void ClearDomainEvents() => DomainEvents.Clear();
}

public class AggregateRoot : AggregateRoot<Guid>
{
    protected AggregateRoot(Guid id) : base(id)
    {
    }
}