namespace Enterprise.Domain.Events.Queuing;

public interface IDomainEventQueue : IEnqueueDomainEvents, IDequeueDomainEvents;
