//using System.Collections.Concurrent;
//using Enterprise.Domain.AspNetCore.Events.Raising;
//using Enterprise.Domain.Events.Model.Abstract;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using static Enterprise.Domain.AspNetCore.Events.Queuing.HttpContextDomainEventQueuingConstants;

//namespace Enterprise.Domain.AspNetCore.Events.Queuing;

//public class HttpContextDomainEventQueueService// : IEnqueueDomainEvents
//{
//    private readonly IHttpContextAccessor? _httpContextAccessor;
//    private readonly ILogger<QueueingDomainEventRaiser> _logger;

//    public HttpContextDomainEventQueueService(IHttpContextAccessor? httpContextAccessor,
//        ILogger<QueueingDomainEventRaiser> logger)
//    {
//        _httpContextAccessor = httpContextAccessor;
//        _logger = logger;
//    }

//    public void Enqueue(IDomainEvent domainEvent) => Enqueue([domainEvent]);

//    public void Enqueue(IReadOnlyCollection<IDomainEvent> domainEvents)
//    {
//        if (!domainEvents.Any())
//            return;

//        if (_httpContextAccessor?.HttpContext == null)
//        {
//            _logger.LogDebug($"{nameof(HttpContext)} is not available. Events will not be queued.");
//            return;
//        }

//        var containsQueue = _httpContextAccessor.HttpContext.Items
//            .TryGetValue(DomainEventQueueKey, out var value);

//        var domainEventQueue = containsQueue && value is ConcurrentQueue<IDomainEvent> existingDomainEvents
//            ? existingDomainEvents
//            : new ConcurrentQueue<IDomainEvent>();

//        foreach (var domainEvent in domainEvents)
//            domainEventQueue.Enqueue(domainEvent);

//        _httpContextAccessor.HttpContext.Items[DomainEventQueueKey] = domainEventQueue;
//    }
//}
