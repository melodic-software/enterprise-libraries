using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events.Raising;

public interface IEnqueueDomainEvents
{
    void Enqueue(IDomainEvent domainEvent);
    void Enqueue(IReadOnlyCollection<IDomainEvent> domainEvents);
}
