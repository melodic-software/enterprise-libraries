using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events.Raising;

/// <summary>
/// Raises domain events.
/// </summary>
public interface IRaiseDomainEvents : IRaiseRecordedDomainEvents
{
    /// <summary>
    /// Raise domain events.
    /// </summary>
    /// <param name="domainEvents"></param>
    /// <returns></returns>
    Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents);

    /// <summary>
    /// Raise domain events.
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    Task RaiseAsync(IDomainEvent domainEvent);
}
