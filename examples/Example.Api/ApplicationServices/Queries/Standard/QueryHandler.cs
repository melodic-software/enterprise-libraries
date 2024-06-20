using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.Events.Facade.Abstract;
using Example.Api.ApplicationServices.Queries.Shared;

namespace Example.Api.ApplicationServices.Queries.Standard;

// Our regular unbound query representation (result is specified by the handler).
// This is an alternative generic - where the result type is bound to the type specified.

public class QueryHandler : QueryHandlerBase<Query, QueryResult>
{
    public QueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(Query query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new QueryResult());
    }
}
