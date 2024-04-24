using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.EventSourcing;

/// <summary>
/// Implementations can apply domain events to themselves, changing their state according to the event's data.
/// This allows for state changes to be driven by the application of (domain) events.
/// </summary>
public interface IApplyDomainEvent
{
    /// <summary>
    /// Applies a domain event.
    /// Implementations should update internal state based on the event's data.
    /// This method is central to the event sourcing pattern, encapsulating the logic for state transitions triggered by events.
    /// </summary>
    /// <param name="domainEvent">The domain event to apply.</param>
    public void Apply(IDomainEvent domainEvent);
}