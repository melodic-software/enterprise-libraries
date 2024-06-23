using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Example.Api.ApplicationServices.Queries.Results;
using MediatR;

namespace Example.Api.ApplicationServices.Queries.Standard.Bound.MediatR;

public class WebApiConfigEventHandlerRegistrar : IRegisterWebApiConfigEventHandlers
{
    public static void RegisterHandlers(WebApiConfigEvents events)
    {
        events.WebApplicationBuilt += async app =>
        {
            await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
            IQueryDispatchFacade queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatchFacade>();
            ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

            var query = new BoundQuery();

            // No generic type parameters are required.
            // The result type is constrained to the generic type defined on the query.
            QueryResult result = await queryDispatcher.DispatchAsync(query, CancellationToken.None);

            // Alternatively MediatR can be used directly.
            // This uses pipeline behaviors instead of decorators or a chain of responsibility.
            QueryResult mediatRResult = await sender.Send(query);
        };
    }
}
