using Enterprise.Domain.Events.Exceptions;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising;
using Enterprise.Patterns.Outbox.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;
using Enterprise.EntityFramework.Outbox;
using Microsoft.Extensions.Logging;
using static Enterprise.EntityFramework.AspNetCore.EventualConsistency.DeferredDomainEventQueueConstants;

namespace Enterprise.EntityFramework.AspNetCore.EventualConsistency;

// NOTE: This middleware has to be registered by each application as the injected DbContext will be application specific.
// TODO: Abstract away the entity framework related types so this can be implemented with other data access technologies.

/// <summary>
/// Middleware that raises queued domain events after the HTTP response has finished being sent to the client.
/// In most cases, the handlers deal with side effects and external integration event publications.
/// Side effects would potentially involve updates to other aggregate roots within the same bounded context.
/// This could be considered a form of eventual consistency within the context boundary.
/// This requires registration of <see cref="DeferredDomainEventInterceptor"/> or the separate use of <see cref="DeferredDomainEventQueueService"/>.
/// Use this OR <see cref="OutboxMessagesInterceptor"/> which internally uses <see cref="OutboxMessagePersistenceService"/>. Do not use BOTH!
/// The alternative approach is in process, the primary difference being that it has to be processed before the response is returned.
/// </summary>
public class DeferredDomainEventRaisingMiddleware<TDbContext> where TDbContext : DbContext
{
    private readonly RequestDelegate _next;
    private readonly IRaiseDomainEvents _eventRaiser;
    private readonly OutboxMessageFactory _outboxMessageFactory;
    private readonly ILogger<DeferredDomainEventRaisingMiddleware<TDbContext>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeferredDomainEventRaisingMiddleware{TDbContext}"/> class.
    /// </summary>
    /// <param name="next">The next request delegate in the middleware pipeline.</param>
    /// <param name="outboxMessageFactory"></param>
    /// <param name="logger"></param>
    /// <param name="eventRaiser"></param>
    public DeferredDomainEventRaisingMiddleware(
        RequestDelegate next,
        IRaiseDomainEvents eventRaiser,
        OutboxMessageFactory outboxMessageFactory,
        ILogger<DeferredDomainEventRaisingMiddleware<TDbContext>> logger)
    {
        _next = next;
        _eventRaiser = eventRaiser;
        _outboxMessageFactory = outboxMessageFactory;
        _logger = logger;
    }

    /// <summary>
    /// Registers a delegate to raise domain events.
    /// The added behavior will only execute when the HTTP response has finished being sent to the client.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="dbContext"></param>
    public async Task InvokeAsync(HttpContext httpContext, TDbContext dbContext)
    {
        _logger.LogInformation("Starting a new database transaction.");

        // NOTE: This is a pragmatic approach as it wraps a transaction around potentially more than one aggregate boundary.
        // Typically, you should only have one transaction per aggregate root, but this is a safe way to ensure
        // all changes are rolled back when errors occur.

        // Manually instructed transactions like this can cause problems
        // if connection resiliency has been configured (auto retry) for transactions.
        // See "EnableRetryOnFailure()".

        IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();

        // Attach a delegate to raise domain events once the HTTP response is completed.
        // We pass the transaction into the delegate, so it won't be disposed when the request ends.
        httpContext.Response.OnCompleted(async () =>
        {
            await ProcessDomainEvents(httpContext, dbContext, transaction);
        });

        // Continue processing the next middleware in the pipeline.
        await _next(httpContext);
    }

    private async Task ProcessDomainEvents(HttpContext httpContext, TDbContext dbContext, IDbContextTransaction transaction)
    {
        try
        {
            // Raise all domain events queued in the context.
            // Here we're typically handling side effects across one or more different aggregate root boundaries within the bounded context.
            // We may also be creating integration events that will be published out to different systems (likely via a transactional outbox).

            bool containsKey = httpContext.Items.ContainsKey(DomainEventQueueKey);

            if (httpContext.Items.TryGetValue(DomainEventQueueKey, out object? value) &&
                value is ConcurrentQueue<IDomainEvent> domainEvents)
            {
                _logger.LogInformation("Processing {DomainEventQueueCount} domain event(s).", domainEvents.Count);

                while (domainEvents.TryDequeue(out IDomainEvent? nextEvent))
                {
                    await _eventRaiser.RaiseAsync(nextEvent);
                }

                _logger.LogInformation("Domain events processed.");

                _logger.LogInformation("Adding domain events as integration events (outbox messages) to the db context.");
                OutboxMessagePersistenceService.AddOutboxMessages(dbContext, domainEvents, _outboxMessageFactory);

                _logger.LogInformation("Saving changes.");
                await dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("No domain events have been queued in this context.");

                if (containsKey)
                {
                    _logger.LogWarning(
                        "HttpContext items contains key: \"{DomainEventQueueKey}\", " +
                        "but the value is not the expected type.",
                        DomainEventQueueKey
                    );
                }
            }

            // Even if we're not processing domain events here, we need to commit the transaction.
            // It was opened at the beginning of the request, so any other EF changes made will not get committed if we don't commit here.
            _logger.LogInformation("Commiting the transaction.");
            await transaction.CommitAsync();

            // Clear the events from the HttpContext to prevent reprocessing.
            httpContext.Items.Remove(DomainEventQueueKey);
        }
        catch (DomainEventHandlingException ex)
        {
            _logger.LogError(ex, "Error processing deferred domain events. The transaction will be rolled back.");

            // Handle exceptions specific to deferred event processing.

            // Depending on the use case, and event handlers,
            // This would involve rolling back ALL changes since the use case has not been fully completed.
            await transaction.RollbackAsync();

            // TODO: Ensure these are handled appropriately.
            // This will require domain event handlers (or a decorator) to
            // wrap any unhandled exceptions in a DomainEventHandlingException instance.

            // The client should probably be notified somehow that this actually ended up failing and was rolled back.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing deferred domain events. The transaction will be rolled back.");
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            // Ensure the transaction is always disposed of.
            await transaction.DisposeAsync();
        }
    }
}