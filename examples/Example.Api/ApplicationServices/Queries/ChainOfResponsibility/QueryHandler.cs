using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.Events.Facade.Abstract;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.ChainOfResponsibility;

public class QueryHandler : QueryHandlerBase<Query, QueryResult>
{
    public QueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(Query query, CancellationToken cancellationToken = default)
    {
        var result = new QueryResult("CHAIN OF RESPONSIBILITY");
        return Task.FromResult(result);
    }
}
