using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.Standard.Bound;

public class WebApiConfigEventHandlerRegistrar : IRegisterWebApiConfigEventHandlers
{
    public static void RegisterHandlers(WebApiConfigEvents events)
    {
        events.WebApplicationBuilt += async app =>
        {
            await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
            IQueryDispatchFacade queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatchFacade>();

            var query = new BoundQuery();

            // No generic type parameters are required.
            // The result type is constrained to the generic type defined on the query.
            QueryResult result = await queryDispatcher.DispatchAsync(query, CancellationToken.None);
        };
    }
}
