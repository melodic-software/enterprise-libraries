using Enterprise.DomainDrivenDesign.Entities.EventRecording;

namespace Enterprise.DomainDrivenDesign.Entities.Aggregates;

// TODO: Do we want to add optimistic concurrency here?
// A version property, and a version incremented property to only increment the version once per transaction?
// Either a state change or domain event being recorded would trigger the version increment.

/// <summary>
/// An aggregate root is an entity that is composed of one or more entities, value objects, etc.
/// This base implementation includes basic functionalities for domain event recording and management.
/// This class should be inherited by any aggregate root in the domain model to ensure proper event handling and state management.
/// </summary>
/// <typeparam name="TId">The type of the identifier for the aggregate root (entity).</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot where TId : IEquatable<TId>
{
    protected AggregateRoot(TId id) : base(id)
    {
    }
}
