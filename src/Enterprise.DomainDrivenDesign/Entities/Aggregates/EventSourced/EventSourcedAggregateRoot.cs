using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.EventSourcing.Entities;
using Enterprise.EventSourcing;

namespace Enterprise.DomainDrivenDesign.Entities.Aggregates.EventSourced;

/// <summary>
/// Extends the aggregate root with event sourcing capabilities.
/// This allows the aggregate's state to be persisted and reconstructed through a sequence of domain events.
/// This class supports operations for loading historical events, taking and applying snapshots, and managing versioning for concurrency control.
/// </summary>
/// <typeparam name="TId">The type of the identifier for the aggregate root.</typeparam>
public abstract class EventSourcedAggregateRoot<TId> : AggregateRoot<TId>, IEventSourcedEntity
    where TId : IEquatable<TId>
{
    // TODO: move the optimistic concurrence properties into a base implementation?
    // See comment on the aggregate root entity.

    /// <summary>
    /// Gets the current version of the aggregate.
    /// This is used for concurrency control and event ordering.
    /// </summary>
    public int Version { get; private set; }

    protected EventSourcedAggregateRoot(TId id) : base(id) { }

    /// <summary>
    /// Reconstitutes the aggregate from a history of domain events.
    /// </summary>
    /// <param name="history">The historical events to load.</param>
    public void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        foreach (IDomainEvent domainEvent in history)
        {
            ApplyChange(domainEvent, false);
        }
    }

    /// <summary>
    /// Takes a snapshot of the aggregate's current state.
    /// </summary>
    /// <returns>A snapshot representing the state of the aggregate.</returns>
    public abstract ISnapshot TakeSnapshot();

    /// <summary>
    /// Applies a snapshot to restore the aggregate's state.
    /// </summary>
    /// <param name="snapshot">The snapshot to apply.</param>
    public abstract void ApplySnapshot(ISnapshot snapshot);

    /// <inheritdoc />
    void IApplyDomainEvent.Apply(IDomainEvent domainEvent) => Apply(domainEvent);

    /// <summary>
    /// Get all state changes that have not yet been committed.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<IDomainEvent> GetUncommittedChanges() => DomainEvents.AsReadOnly();

    /// <summary>
    /// Clear changes.
    /// </summary>
    public void MarkChangesAsCommitted() => DomainEvents.Clear();

    /// <summary>
    /// Applies a domain event to the aggregate's state.
    /// This method is intended to be used internally by the aggregate to update its state in response to an event.
    /// It should be called only by the ApplyChange method or when replaying events from the event store during the rehydration of an aggregate.
    /// Derived classes must implement this method to define how state changes in response to each specific type of event.
    /// </summary>
    /// <param name="domainEvent">The event to apply to the aggregate.</param>
    protected abstract void Apply(IDomainEvent domainEvent);

    /// <inheritdoc />
    protected override void Record(IDomainEvent domainEvent)
    {
        ApplyChange(domainEvent);
    }

    /// <summary>
    /// Applies a domain event to the aggregate and optionally records it as an uncommitted change.
    /// This method is the primary way to handle new events within the aggregate.
    /// Use this method for new events that result from processing commands or other actions that change the state of the aggregate.
    /// </summary>
    /// <param name="domainEvent">The event to apply.</param>
    /// <param name="isNewEvent">Indicates whether the event is new and should be recorded.
    /// Set this to true when the event originates from within the aggregate as a result of state changes.
    /// Set to false when replaying historical events to rehydrate the aggregate's state.</param>
    protected void ApplyChange(IDomainEvent domainEvent, bool isNewEvent = true)
    {
        // Prevent applying the same change more than once.
        if (DomainEvents.Any(c => c.Id == domainEvent.Id))
        {
            return;
        }

        Apply(domainEvent);

        if (!isNewEvent)
        {
            return;
        }

        DomainEvents.Add(domainEvent);

        Version++;
    }
}
