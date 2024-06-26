using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;
using Enterprise.DI.Core.Registration.Attributes;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.ChainOfResponsibility;

[ExcludeRegistrations]
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

            // This should not be used directly, but the underlying type is registered and used internally.
            IResponsibilityChain<Query, QueryResult> responsibilityChain = scope.ServiceProvider.GetRequiredService<IResponsibilityChain<Query, QueryResult>>();
            QueryResult? result2 = await responsibilityChain.HandleAsync(query, CancellationToken.None);
        };
    }
}
