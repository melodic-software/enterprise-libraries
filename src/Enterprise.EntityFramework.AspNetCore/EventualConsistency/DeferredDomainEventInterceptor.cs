using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.EventualConsistency;

public class DeferredDomainEventInterceptor : SaveChangesInterceptor
{
    private readonly ILogger<DeferredDomainEventInterceptor> _logger;
    private readonly DeferredDomainEventQueueService _deferredDomainEventQueueService;

    public DeferredDomainEventInterceptor(ILogger<DeferredDomainEventInterceptor> logger,
        DeferredDomainEventQueueService deferredDomainEventQueueService)
    {
        _logger = logger;
        _deferredDomainEventQueueService = deferredDomainEventQueueService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context != null)
            _deferredDomainEventQueueService.AddEventsToQueue(eventData.Context, _logger);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = new())
    {
        if (eventData.Context != null)
            _deferredDomainEventQueueService.AddEventsToQueue(eventData.Context, _logger);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}