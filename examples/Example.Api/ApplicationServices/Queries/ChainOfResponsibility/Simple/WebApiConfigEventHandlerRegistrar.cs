using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.ChainOfResponsibility.Simple;

public class WebApiConfigEventHandlerRegistrar : IRegisterWebApiConfigEventHandlers
{
    public static void RegisterHandlers(WebApiConfigEvents events)
    {
        events.WebApplicationBuilt += async app =>
        {
            await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
            IQueryDispatchFacade queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatchFacade>();

            var query = new Query();

            QueryResult result = await queryDispatcher.DispatchAsync<Query, QueryResult>(query, CancellationToken.None);
            
            IHandleQuery<Query, QueryResult> queryHandler = scope.ServiceProvider.GetRequiredService<IHandleQuery<Query, QueryResult>>();
            QueryResult result2 = await queryHandler.HandleAsync(query, CancellationToken.None);
        };
    }
}
