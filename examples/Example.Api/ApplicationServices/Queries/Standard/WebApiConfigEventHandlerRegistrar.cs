using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Example.Api.ApplicationServices.Queries.Shared;
using Example.Api.Events;
using Unbound = Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;

namespace Example.Api.ApplicationServices.Queries.Standard;

public class WebApiConfigEventHandlerRegistrar : IRegisterWebApiConfigEventHandlers
{
    public static void RegisterHandlers(WebApiConfigEvents events)
    {
        events.WebApplicationBuilt += async app =>
        {
            await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
            IQueryDispatchFacade queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatchFacade>();

            var query = new Query();

            // Generic type parameters are required because the result type is not bound to the query itself.
            queryDispatcher.RegisterEventCallback(new Action<MyEvent>(@event =>
            {

            }));

            QueryResult result = await queryDispatcher.DispatchAsync<Query, QueryResult>(query, CancellationToken.None);
            
            IHandleQuery<Query, QueryResult> queryHandler = scope.ServiceProvider.GetRequiredService<IHandleQuery<Query, QueryResult>>();
            QueryResult result1 = await queryHandler.HandleAsync(query, CancellationToken.None);

            Unbound.IHandleQuery<QueryResult> unboundQueryHandler = scope.ServiceProvider.GetRequiredService<Unbound.IHandleQuery<QueryResult>>();
            QueryResult result3 = await unboundQueryHandler.HandleAsync(query, CancellationToken.None);

            IEnumerable<Unbound.IHandleQuery<QueryResult>> allUnboundHandlers = scope.ServiceProvider.GetServices<Unbound.IHandleQuery<QueryResult>>();
            IEnumerable<IHandleQuery> allNonGenericHandlers = scope.ServiceProvider.GetServices<IHandleQuery>();
        };
    }
}
