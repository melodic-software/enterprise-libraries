using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Queuing;
using Enterprise.Domain.Events.Raising.Abstract;

namespace Enterprise.Domain.Events.Raising;

public class QueueingDomainEventRaiser : IRaiseDomainEvents
{
    private readonly IEnqueueDomainEvents _queueService;

    public QueueingDomainEventRaiser(IEnqueueDomainEvents queueService)
    {
        _queueService = queueService;
    }

    public Task RaiseAsync(IEnumerable<IGetDomainEvents> entities)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = entities
            .SelectMany(x => x.GetDomainEvents())
            .ToList()
            .AsReadOnly();

        _queueService.Enqueue(domainEvents);

        return Task.CompletedTask;
    }

    public Task RaiseAsync(IGetDomainEvents entity)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = entity
            .GetDomainEvents()
            .ToList()
            .AsReadOnly();

        _queueService.Enqueue(domainEvents);

        return Task.CompletedTask;
    }

    public Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        _queueService.Enqueue(domainEvents);
        return Task.CompletedTask;
    }

    public Task RaiseAsync(IDomainEvent domainEvent)
    {
        _queueService.Enqueue(domainEvent);
        return Task.CompletedTask;
    }
}
