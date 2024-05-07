using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events.Queuing;

public interface IDequeueDomainEvents
{
    IDomainEvent? Dequeue();
}
