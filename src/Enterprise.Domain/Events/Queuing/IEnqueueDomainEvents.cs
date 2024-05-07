using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events.Queuing;

public interface IEnqueueDomainEvents
{
    void Enqueue(IDomainEvent domainEvent);
    void Enqueue(IReadOnlyCollection<IDomainEvent> domainEvents);
}
