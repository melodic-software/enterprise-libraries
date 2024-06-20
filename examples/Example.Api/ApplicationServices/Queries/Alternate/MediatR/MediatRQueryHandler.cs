using Enterprise.Events.Facade.Abstract;
using Enterprise.MediatR.Queries.Handlers;
using Example.Api.ApplicationServices.Queries.Shared;

namespace Example.Api.ApplicationServices.Queries.Alternate.MediatR;

public class MediatRQueryHandler : MediatRQueryHandlerBase<AlternateQuery, QueryResult>
{
    public MediatRQueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(AlternateQuery query, CancellationToken cancellationToken = new())
    {
        return Task.FromResult(new QueryResult());
    }
}
