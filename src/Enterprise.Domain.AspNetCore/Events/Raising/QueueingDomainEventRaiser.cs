//using Enterprise.Domain.Events;
//using Enterprise.Domain.Events.Model.Abstract;
//using Enterprise.Domain.Events.Raising;
//using Enterprise.Events.Raising.Callbacks.Abstractions;

//namespace Enterprise.Domain.AspNetCore.Events.Raising;

//public class QueueingDomainEventRaiser : IRaiseDomainEvents
//{
//    private readonly IEnqueueDomainEvents _queueService;

//    public QueueingDomainEventRaiser(IEnqueueDomainEvents queueService)
//    {
//        _queueService = queueService;
//    }

//    public Task RaiseAsync(IEnumerable<IGetDomainEvents> entities, IRaiseEventCallbacks? callbackService = null)
//    {
//        IReadOnlyCollection<IDomainEvent> domainEvents = entities
//            .SelectMany(x => x.GetDomainEvents())
//            .ToList()
//            .AsReadOnly();

//        _queueService.Enqueue(domainEvents);

//        callbackService?.RaiseCallbacks(domainEvents);

//        return Task.CompletedTask;
//    }

//    public Task RaiseAsync(IGetDomainEvents entity, IRaiseEventCallbacks? callbackService = null)
//    {
//        IReadOnlyCollection<IDomainEvent> domainEvents = entity.GetDomainEvents().ToList().AsReadOnly();
//        _queueService.Enqueue(domainEvents);
//        callbackService?.RaiseCallbacks(domainEvents);
//        return Task.CompletedTask;
//    }

//    public Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents, IRaiseEventCallbacks? callbackService = null)
//    {
//        _queueService.Enqueue(domainEvents);
//        callbackService?.RaiseCallbacks(domainEvents);
//        return Task.CompletedTask;
//    }

//    public Task RaiseAsync(IDomainEvent domainEvent, IRaiseEventCallbacks? callbackService = null)
//    {
//        _queueService?.Enqueue(domainEvent);
//        callbackService?.RaiseCallbacks(domainEvent);
//        return Task.CompletedTask;
//    }
//}
