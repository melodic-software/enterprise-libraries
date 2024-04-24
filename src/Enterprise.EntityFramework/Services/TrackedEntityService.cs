using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.DomainDrivenDesign.Entities.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Services;

public static class TrackedEntityService
{
    /// <summary>
    /// Aggregate domain events from aggregate root entities currently being tracked by the db context.
    /// By default, events are cleared after retrieval to prevent potential errors involving duplicate events.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="clearDomainEvents"></param>
    /// <returns></returns>
    public static IReadOnlyCollection<IDomainEvent> GetDomainEventsFromTrackedEntities(DbContext dbContext, ILogger logger, bool clearDomainEvents = true)
    {
        // These should be aggregate root entities that internally record events that have occurred within the aggregate boundary.
        List<EntityEntry<IAggregateRoot>> aggregateRoots = dbContext.ChangeTracker.Entries<IAggregateRoot>().ToList();

        if (!aggregateRoots.Any())
            return new List<IDomainEvent>();

        int aggregateRootCount = aggregateRoots.Count;

        logger.LogInformation(
            "Collecting " +
            (clearDomainEvents ? "and clearing " : string.Empty) +
            "domain events from {AggregateRootCount} aggregate root(s).",
            aggregateRootCount
        );

        List<IDomainEvent> domainEvents = aggregateRoots
            .Select(e =>
            {
                IReadOnlyList<IDomainEvent> domainEvents = e.Entity.GetDomainEvents();

                // Have to clear these, so they don't get reprocessed and cause issues downstream.
                // This can cause problems depending on the DI lifetime of the db context.
                if (clearDomainEvents)
                    e.Entity.ClearDomainEvents();

                return domainEvents;
            })
            .SelectMany(x => x)
            .ToList();

        logger.LogInformation(
            "Collected " +
            (clearDomainEvents ? "and cleared " : string.Empty) +
            "{DomainEventCount} domain events from {AggregateRootCount} aggregate root(s).",
            domainEvents.Count, aggregateRootCount
        );

        return domainEvents;
    }
}