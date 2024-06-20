using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Example.Api.ApplicationServices.Queries.Shared;
using Bound = Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Unbound = Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;

namespace Example.Api.ApplicationServices.Queries.Alternate.MediatR;

public class EventHandlerRegistrar : IRegisterWebApiConfigEventHandlers
{
    public static void RegisterHandlers(WebApiConfigEvents events)
    {
        events.WebApplicationBuilt += async app =>
        {
            await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
            IQueryDispatchFacade queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatchFacade>();

            var query = new AlternateQuery();

            // No generic type parameters are required.
            // The result type is constrained to the generic type defined on the query.
            QueryResult result = await queryDispatcher.DispatchAsync(query, CancellationToken.None);
            QueryResult alternateResult = await queryDispatcher.DispatchAsync<AlternateQuery, QueryResult>(query, CancellationToken.None);

            IHandleQuery<AlternateQuery, QueryResult> queryHandler = scope.ServiceProvider.GetRequiredService<IHandleQuery<AlternateQuery, QueryResult>>();
            QueryResult result1 = await queryHandler.HandleAsync(query, CancellationToken.None);

            Bound.IHandleQuery<AlternateQuery, QueryResult> boundQueryHandler = scope.ServiceProvider.GetRequiredService<Bound.IHandleQuery<AlternateQuery, QueryResult>>();
            QueryResult result2 = await boundQueryHandler.HandleAsync(query, CancellationToken.None);

            Unbound.IHandleQuery<QueryResult> unboundQueryHandler = scope.ServiceProvider.GetRequiredService<Unbound.IHandleQuery<QueryResult>>();
            QueryResult result3 = await unboundQueryHandler.HandleAsync(query, CancellationToken.None);

            IEnumerable<Unbound.IHandleQuery<QueryResult>> allUnboundHandlers = scope.ServiceProvider.GetServices<Unbound.IHandleQuery<QueryResult>>();
        };
    }
}
