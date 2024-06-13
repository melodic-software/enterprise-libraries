using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Queuing;
using Enterprise.EntityFramework.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.EventualConsistency;

public class DomainEventQueuingInterceptor : SaveChangesInterceptor
{
    private readonly IEnqueueDomainEvents _domainEventQueueService;
    private readonly ILogger<DomainEventQueuingInterceptor> _logger;

    public DomainEventQueuingInterceptor(IEnqueueDomainEvents domainEventQueueService, ILogger<DomainEventQueuingInterceptor> logger)
    {
        _logger = logger;
        _domainEventQueueService = domainEventQueueService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context != null)
        {
            IReadOnlyCollection<IDomainEvent> domainEvents = TrackedEntityService
                .GetDomainEventsFromTrackedEntities(eventData.Context, _logger);

            _domainEventQueueService.Enqueue(domainEvents);
        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            IReadOnlyCollection<IDomainEvent> domainEvents = TrackedEntityService
                .GetDomainEventsFromTrackedEntities(eventData.Context, _logger);

            _domainEventQueueService.Enqueue(domainEvents);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
