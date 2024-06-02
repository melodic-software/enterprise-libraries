using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Queries.Handlers.Simple;
using Enterprise.Events.Facade.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Simple;

internal static class SimpleQueryHandlerFactoryService
{
    internal static Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>>
        GetImplementationFactory<TQuery, TResponse>() where TQuery : IBaseQuery
    {
        return provider =>
        {
            IEventRaisingFacade eventRaisingFacade = provider.GetRequiredService<IEventRaisingFacade>();

            // Resolve the query logic implementation.
            IQueryLogic<TQuery, TResponse> queryLogic = provider.GetRequiredService<IQueryLogic<TQuery, TResponse>>();

            // Use a common handler that delegates to the query logic.
            // We can still add cross-cutting concerns and decorate this handler as needed.
            return new SimpleQueryHandler<TQuery, TResponse>(eventRaisingFacade, queryLogic);
        };
    }
}
