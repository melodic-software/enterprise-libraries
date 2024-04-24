using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events;

/// <summary>
/// Allows implementations to expose a collection of domain events.
/// </summary>
public interface IGetDomainEvents
{
    /// <summary>
    /// Get the collection of domain events.
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<IDomainEvent> GetDomainEvents();
}