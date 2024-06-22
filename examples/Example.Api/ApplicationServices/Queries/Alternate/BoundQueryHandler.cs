using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.Events.Facade.Abstract;
using Example.Api.ApplicationServices.Queries.Shared;

namespace Example.Api.ApplicationServices.Queries.Alternate;

public class BoundQueryHandler : QueryHandlerBase<AlternateQuery, QueryResult>
{
    public BoundQueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(AlternateQuery query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new QueryResult());
    }
}
