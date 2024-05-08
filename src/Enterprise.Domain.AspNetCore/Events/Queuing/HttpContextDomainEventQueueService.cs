using System.Collections.Concurrent;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Queuing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.AspNetCore.Events.Queuing;

public class HttpContextDomainEventQueueService : IDomainEventQueue
{
    private const string DomainEventsQueueKey = "DomainEventsQueue";

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

        _logger.LogInformation("Attempting to queue {DomainEventCount} domain event(s).", domainEvents.Count);

        bool containsQueue = _httpContextAccessor.HttpContext.Items
            .TryGetValue(DomainEventsQueueKey, out object? value);

        ConcurrentQueue<IDomainEvent> domainEventQueue = containsQueue && value is ConcurrentQueue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new ConcurrentQueue<IDomainEvent>();

        _logger.LogInformation("Domain event queue currently contains {QueuedDomainEventCount} domain event(s).", domainEventQueue.Count);

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            Enqueue(domainEvent, domainEventQueue);
        }

        _httpContextAccessor.HttpContext.Items[DomainEventsQueueKey] = domainEventQueue;
    }

    private void Enqueue(IDomainEvent domainEvent, ConcurrentQueue<IDomainEvent> domainEventQueue)
    {
        using (_logger.BeginScope("Domain Event Type: {DomainEventType}, Domain Event Id: {DomainEventId}", domainEvent.GetType().Name, domainEvent.Id))
        {
            bool alreadyQueued = domainEventQueue.Any(x => x.Id == domainEvent.Id);

            if (alreadyQueued)
            {
                _logger.LogInformation("Domain event has already been queued.");
                return;
            }
            
            domainEventQueue.Enqueue(domainEvent);
            _logger.LogInformation("Domain event has been successfully queued.");
        }
    }

    public IDomainEvent? Dequeue()
    {
        if (_httpContextAccessor?.HttpContext == null)
        {
            _logger.LogDebug($"{nameof(HttpContext)} is not available. Events cannot be dequeued.");
            return null;
        }

        bool containsQueue = _httpContextAccessor.HttpContext.Items.TryGetValue(DomainEventsQueueKey, out object? value);

        ConcurrentQueue<IDomainEvent> domainEventQueue = containsQueue && value is ConcurrentQueue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new ConcurrentQueue<IDomainEvent>();

        if (domainEventQueue.IsEmpty)
        {
            _logger.LogInformation("There are no domain events in the queue.");
            return null;
        }

        _logger.LogInformation(
            "Domain event queue currently contains {QueuedDomainEventCount} domain event(s). " +
            "Attempting to dequeue domain event.",
            domainEventQueue.Count
        );

        bool elementRemoved = domainEventQueue.TryDequeue(out IDomainEvent? result);

        LogDequeueResult(elementRemoved, result);

        return result;
    }

    private void LogDequeueResult(bool elementRemoved, IDomainEvent? result)
    {
        if (elementRemoved)
        {
            _logger.LogInformation("Domain event dequeued successfully.");

            if (result != null)
                return;

            _logger.LogWarning("Dequeued domain event is null.");
        }
        else
        {
            _logger.LogInformation("No item to dequeue.");
        }
    }
}
