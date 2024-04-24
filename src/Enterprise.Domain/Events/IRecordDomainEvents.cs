using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events;

/// <summary>
/// Implementations allow for public recording of domain events.
/// Some paradigms like domain driven design suggest that this functionality should not be publicly accessible.
/// In those cases, this interface should not be used. The recording should be encapsulated internally and triggered by use case specific methods.
/// </summary>
public interface IRecordDomainEvents
{
    public void Record(IDomainEvent domainEvent);
}