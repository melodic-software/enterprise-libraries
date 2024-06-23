
using Enterprise.Events.Facade.Abstract;
using Enterprise.MediatR.Queries.Handlers.Bound;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.Standard.Bound.MediatR;

public class MediatRQueryHandler : MediatRQueryHandlerBase<BoundQuery, QueryResult>
{
    public MediatRQueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(BoundQuery query, CancellationToken cancellationToken = new())
    {
        var result = new QueryResult("MEDIATR");
        return Task.FromResult(result);
    }
}
