using Enterprise.Domain.Entities;
using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.EventSourcing.Entities;

/// <summary>
/// Defines the interface for entities that use event sourcing for state management
/// Such entities maintain their state by applying a sequence of events and can reconstitute their state from these events.
/// It extends basic entity capabilities with methods specific to event sourcing.
/// These include loading from history, applying events as state changes, and managing uncommitted changes.
/// </summary>

/// <summary>
/// An event-sourced entity combines the capabilities of a domain entity with event application and history management.
/// </summary>
public interface IEventSourcedEntity : IEntity, IApplyDomainEvent
{
    /// <summary>
    /// Loads the entity's state from a historical sequence of domain events.
    /// This method is used to rehydrate an entity to its current state by replaying its event history.
    /// </summary>
    /// <param name="history">The enumerable collection of domain events.</param>
    void LoadFromHistory(IEnumerable<IDomainEvent> history);

    /// <summary>
    /// Takes a snapshot of the aggregate's current state.
    /// </summary>
    /// <returns>A snapshot representing the state of the aggregate.</returns>
    public ISnapshot TakeSnapshot();

    /// <summary>
    /// Applies a snapshot to restore the aggregate's state.
    /// </summary>
    /// <param name="snapshot">The snapshot to apply.</param>
    public void ApplySnapshot(ISnapshot snapshot);

    /// <summary>
    /// Retrieves a read-only list of uncommitted domain events.
    /// These are events that have occurred within the entity since the last time changes were committed.
    /// </summary>
    /// <returns>A read-only list of uncommitted domain events.</returns>
    IReadOnlyList<IDomainEvent> GetUncommittedChanges();

    /// <summary>
    /// Marks all uncommitted domain events as committed, effectively clearing the list of uncommitted events.
    /// This is typically called after the events have been successfully persisted.
    /// </summary>
    void MarkChangesAsCommitted();
}