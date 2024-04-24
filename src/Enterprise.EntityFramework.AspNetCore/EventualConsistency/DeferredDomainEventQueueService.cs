using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.EntityFramework.Services;
using static Enterprise.EntityFramework.AspNetCore.EventualConsistency.DeferredDomainEventQueueConstants;

namespace Enterprise.EntityFramework.AspNetCore.EventualConsistency;

// TODO: Abstract away the entity framework related types so this can be implemented with other data access technologies.

public class DeferredDomainEventQueueService
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly ILogger<DeferredDomainEventQueueService> _logger;

    public DeferredDomainEventQueueService(
        IHttpContextAccessor? httpContextAccessor,
        ILogger<DeferredDomainEventQueueService> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public void AddEventsToQueue(DbContext dbContext, ILogger logger)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = TrackedEntityService
            .GetDomainEventsFromTrackedEntities(dbContext, logger);

        AddEventsToQueue(domainEvents);
    }

    public void AddEventsToQueue(IReadOnlyCollection<IDomainEvent> domainEvents)
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