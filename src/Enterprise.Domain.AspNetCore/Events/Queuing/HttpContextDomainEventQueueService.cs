using System.Collections.Concurrent;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Queuing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static Enterprise.Domain.Events.Queuing.DomainEventQueuingConstants;

namespace Enterprise.Domain.AspNetCore.Events.Queuing;

public class HttpContextDomainEventQueueService : IEnqueueDomainEvents
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly ILogger<HttpContextDomainEventQueueService> _logger;

    public HttpContextDomainEventQueueService(IHttpContextAccessor? httpContextAccessor,
        ILogger<HttpContextDomainEventQueueService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public void Enqueue(IDomainEvent domainEvent) => Enqueue([domainEvent]);

    public void Enqueue(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        if (!domainEvents.Any())
            return;

        if (_httpContextAccessor?.HttpContext == null)
        {
            _logger.LogDebug($"{nameof(HttpContext)} is not available. Events will not be queued.");
            return;
        }

        bool containsQueue = _httpContextAccessor.HttpContext.Items
            .TryGetValue(DomainEventQueueKey, out object? value);

        ConcurrentQueue<IDomainEvent> domainEventQueue = containsQueue && value is ConcurrentQueue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new ConcurrentQueue<IDomainEvent>();

        foreach (IDomainEvent domainEvent in domainEvents)
            domainEventQueue.Enqueue(domainEvent);

        _httpContextAccessor.HttpContext.Items[DomainEventQueueKey] = domainEventQueue;
    }
}
