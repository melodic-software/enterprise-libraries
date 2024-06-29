using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.DomainDrivenDesign.Entities.EventRecording;

public abstract class Entity<TId> : Standard.Entity<TId>, IEventRecordingEntity where TId : IEquatable<TId>
{
    /// <summary>
    /// This is the collection of internal domain events.
    /// These are changes that have been made and recorded within the aggregate root.
    /// </summary>
    protected readonly List<IDomainEvent> DomainEvents = [];

    protected Entity(TId id) : base(id)
    {
    }

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => DomainEvents.ToList().AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents() => DomainEvents.Clear();

    /// <summary>
    /// Record a domain event.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected virtual void Record(IDomainEvent domainEvent)
    {
        // Prevent recording of duplicate events.
        if (DomainEvents.Any(x => x.Id == domainEvent.Id))
        {
            return;
        }

        DomainEvents.Add(domainEvent);
    }
}
