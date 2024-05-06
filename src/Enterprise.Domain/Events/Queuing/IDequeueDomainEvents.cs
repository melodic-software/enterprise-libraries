using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events.Raising;

public interface IDequeueDomainEvents
{
    IDomainEvent? Dequeue();
}
