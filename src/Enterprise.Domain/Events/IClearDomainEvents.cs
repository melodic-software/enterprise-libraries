namespace Enterprise.Domain.Events;

/// <summary>
/// Provides a mechanism for clearing domain events.
/// </summary>
public interface IClearDomainEvents
{
    /// <summary>
    /// Clear the collection of domain events.
    /// </summary>
    void ClearDomainEvents();
}