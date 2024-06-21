using Enterprise.Events.Facade.Abstract;
using Enterprise.MediatR.Queries.Handlers.Bound;
using Example.Api.ApplicationServices.Queries.Shared;
using Example.Api.Events;

namespace Example.Api.ApplicationServices.Queries.Alternate.MediatR;

public class MediatRQueryHandler : MediatRQueryHandlerBase<AlternateQuery, QueryResult>
{
    public MediatRQueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(AlternateQuery query, CancellationToken cancellationToken = new())
    {
        var @event = new MyEvent();
        RaiseEventAsync(@event);
        return Task.FromResult(new QueryResult());
    }
}
