using System.Collections.Concurrent;
using Enterprise.Domain.Events.Exceptions;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Patterns.Outbox.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static Enterprise.Domain.Events.Queuing.DomainEventQueuingConstants;

namespace Enterprise.Domain.AspNetCore.Events.Queuing.Middleware;

// TODO: As this is defined, have the DeferredDomainEventRaisingMiddleware in the EntityFramework project derive from this and reuse as much as possible.

/// <summary>
/// Middleware that raises queued domain events after the HTTP response has finished being sent to the client.
/// In most cases, the handlers deal with side effects and external integration event publications.
/// Side effects would potentially involve updates to other aggregate roots within the same bounded context.
/// This could be considered a form of eventual consistency within the context boundary.
/// The alternative approach is in process, the primary difference being that it has to be processed before the response is returned.
/// </summary>
public class DeferredDomainEventRaisingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly EventOutboxMessageFactory _outboxMessageFactory;
    private readonly ILogger<DeferredDomainEventRaisingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeferredDomainEventRaisingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next request delegate in the middleware pipeline.</param>
    /// <param name="outboxMessageFactory"></param>
    /// <param name="logger"></param>
    public DeferredDomainEventRaisingMiddleware(
        RequestDelegate next,
        EventOutboxMessageFactory outboxMessageFactory,
        ILogger<DeferredDomainEventRaisingMiddleware> logger)
    {
        _next = next;
        _outboxMessageFactory = outboxMessageFactory;
        _logger = logger;
    }

    /// <summary>
    /// Registers a delegate to raise domain events.
    /// The added behavior will only execute when the HTTP response has finished being sent to the client.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="eventRaiser"></param>
    public async Task InvokeAsync(HttpContext httpContext, IRaiseQueuedDomainEvents eventRaiser)
    {
        //_logger.LogInformation("Starting a new database transaction.");

        // NOTE: This is a pragmatic approach as it wraps a transaction around potentially more than one aggregate boundary.
        // Typically, you should only have one transaction per aggregate root, but this is a safe way to ensure
        // all changes are rolled back when errors occur.

        // Manually instructed transactions like this can cause problems
        // if connection resiliency has been configured (auto retry) for transactions.
        // See "EnableRetryOnFailure()".

        // TODO: Figure out how to add abstract transaction here that can work at least EntityFramework or Dapper (preferably more).
        //IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();

        // Attach a delegate to raise domain events once the HTTP response is completed.
        // TODO: We need to pass the transaction into the delegate, so it won't be disposed when the request ends.
        httpContext.Response.OnCompleted(async () =>
        {
            await ProcessDomainEventsAsync(httpContext, eventRaiser);
        });

        // Continue processing the next middleware in the pipeline.
        await _next(httpContext);
    }

    private async Task ProcessDomainEventsAsync(HttpContext httpContext, IRaiseQueuedDomainEvents eventRaiser)
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
                    await eventRaiser.RaiseAsync(nextEvent);
                }

                _logger.LogInformation("Domain events processed.");

                //_logger.LogInformation("Adding domain events as integration events (outbox messages).");
                //OutboxMessagePersistenceService.AddOutboxMessages(domainEvents, _outboxMessageFactory);

                //_logger.LogInformation("Saving changes.");
                // TODO: Add abstraction that can save changes. It needs to work with at least EntityFramework or Dapper (preferably more)
                //await dbContext.SaveChangesAsync();
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
            //_logger.LogInformation("Commiting the transaction.");
            // TODO: Provide and use transaction abstraction and commit it here. The implementations will need to support both EntityFramework and Dapper (but preferably more).
            //await transaction.CommitAsync();

            // Clear the events from the HttpContext to prevent reprocessing.
            httpContext.Items.Remove(DomainEventQueueKey);
        }
        catch (DomainEventHandlingException ex)
        {
            _logger.LogError(ex, "Error processing deferred domain events. The transaction will be rolled back.");

            // Handle exceptions specific to deferred event processing.

            // Depending on the use case, and event handlers,
            // This would involve rolling back ALL changes since the use case has not been fully completed.
            // TODO: Provide and use transaction abstraction and rollback here. The implementations will need to support both EntityFramework and Dapper (but preferably more).
            //await transaction.RollbackAsync();

            // TODO: Ensure these are handled appropriately.
            // This will require domain event handlers (or a decorator) to
            // wrap any unhandled exceptions in a DomainEventHandlingException instance.

            // The client should probably be notified somehow that this actually ended up failing and was rolled back.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing deferred domain events. The transaction will be rolled back.");
            // TODO: Provide and use transaction abstraction and rollback here. The implementations will need to support both EntityFramework and Dapper (but preferably more).
            //await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            // Ensure the transaction is always disposed of.
            // TODO: Provide and use transaction abstraction and dispose here. The implementations will need to support both EntityFramework and Dapper (but preferably more).
            //await transaction.DisposeAsync();
        }
    }
}
