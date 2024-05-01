using Enterprise.Patterns.Outbox.Factory;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Outbox;

public sealed class OutboxMessagesInterceptor : SaveChangesInterceptor
{
    private readonly ILogger<OutboxMessagesInterceptor> _logger;
    private readonly EventOutboxMessageFactory _outboxMessageFactory;

    public OutboxMessagesInterceptor(ILogger<OutboxMessagesInterceptor> logger, EventOutboxMessageFactory outboxMessageFactory)
    {
        _logger = logger;
        _outboxMessageFactory = outboxMessageFactory;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
            InsertOutboxMessages(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            InsertOutboxMessages(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void InsertOutboxMessages(DbContext dbContext)
    {
        OutboxMessagePersistenceService.AddDomainEventsAsOutboxMessages(dbContext, _logger, _outboxMessageFactory);
    }
}
