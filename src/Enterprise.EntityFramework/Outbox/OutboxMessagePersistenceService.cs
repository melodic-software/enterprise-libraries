using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Patterns.Outbox.Factory;
using Enterprise.Patterns.Outbox.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Enterprise.EntityFramework.Services.TrackedEntityService;

namespace Enterprise.EntityFramework.Outbox;

/// <summary>
/// Adds domain events as outbox messages.
/// We should only write events to the outbox that downstream systems care about.
/// The integration event object should also be separated from the domain event.
/// </summary>
public static class OutboxMessagePersistenceService
{
    /// <summary>
    /// Add any domain events recorded on entity objects as outbox messages in the same transaction.
    /// This only adds the events as outbox messages. A separate call should be made to save changes.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="outboxMessageFactory"></param>
    public static IReadOnlyCollection<OutboxMessage> AddDomainEventsAsOutboxMessages(DbContext dbContext, ILogger logger, OutboxMessageFactory outboxMessageFactory)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = GetDomainEventsFromTrackedEntities(dbContext, logger);
        IReadOnlyCollection<OutboxMessage> outboxMessages = AddOutboxMessages(dbContext, domainEvents, outboxMessageFactory);
        return outboxMessages;
    }

    /// <summary>
    /// Add any domain events as outbox messages in the same transaction.
    /// This only adds the events as outbox messages. A separate call should be made to save changes.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="domainEvents"></param>
    /// <param name="outboxMessageFactory"></param>
    public static IReadOnlyCollection<OutboxMessage> AddOutboxMessages(DbContext dbContext, IReadOnlyCollection<IDomainEvent> domainEvents, OutboxMessageFactory outboxMessageFactory)
    {
        // Create outbox messages from the domain event collection.
        IReadOnlyCollection<OutboxMessage> outboxMessages = outboxMessageFactory.CreateFrom(domainEvents);

        // Add these so they will be persisted.
        dbContext.AddRange(outboxMessages);

        return outboxMessages;
    }
}