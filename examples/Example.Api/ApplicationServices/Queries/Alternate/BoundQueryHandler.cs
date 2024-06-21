using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.Events.Facade.Abstract;
using Example.Api.ApplicationServices.Queries.Shared;
using Example.Api.Events;

namespace Example.Api.ApplicationServices.Queries.Alternate;

public class BoundQueryHandler : QueryHandlerBase<AlternateQuery, QueryResult>
{
    public BoundQueryHandler(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public override Task<QueryResult> HandleAsync(AlternateQuery query, CancellationToken cancellationToken = default)
    {
        var @event = new MyEvent();
        RaiseEventAsync(@event);
        return Task.FromResult(new QueryResult());
    }
}
