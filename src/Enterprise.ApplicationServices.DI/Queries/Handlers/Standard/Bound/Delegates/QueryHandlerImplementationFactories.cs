using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Enterprise.ApplicationServices.Queries.Handlers.Simple.Bound;
using Enterprise.Events.Facade.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Bound.Delegates;

public static class QueryHandlerImplementationFactories
{
    public static QueryHandlerBase<TQuery, TResult> CreateSimpleQueryHandler<TQuery, TResult>(IServiceProvider provider)
        where TQuery : class, IQuery<TResult>
    {
        IEventRaisingFacade eventRaisingFacade = provider.GetRequiredService<IEventRaisingFacade>();

        // Resolve the query logic implementation.
        IQueryLogic<TQuery, TResult> queryLogic = provider.GetRequiredService<IQueryLogic<TQuery, TResult>>();

        // Use a common handler that delegates to the query logic.
        // We can still add cross-cutting concerns and decorate this handler as needed.
        return new SimpleQueryHandler<TQuery, TResult>(eventRaisingFacade, queryLogic);
    }
}
