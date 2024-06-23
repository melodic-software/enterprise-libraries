using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.Events.Facade.Abstract;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.Standard.Bound;

public class BoundQueryHandler : QueryHandlerBase<BoundQuery, QueryResult>
{
    public BoundQueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(BoundQuery query, CancellationToken cancellationToken = default)
    {
        var result = new QueryResult("BOUND QUERY HANDLER");
        return Task.FromResult(result);
    }
}
