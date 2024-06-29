using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;
using Enterprise.Events.Facade.Abstract;

namespace Enterprise.ApplicationServices.Queries.Handlers.Simple.Unbound;

/// <summary>
/// Most query handler implementations end up being pretty thin...
/// Some pragmatic approaches involve writing the data access code directly in the handler, but this violates clean architecture.
/// One solution is to use a prebuilt handler implementation which requires a query logic implementation.
/// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class SimpleQueryHandler<TQuery, TResult> :
    QueryHandlerBase<TQuery, TResult>
    where TQuery : class, IQuery
{
    private readonly IQueryLogic<TQuery, TResult> _queryLogic;

    /// <summary>
    /// Most query handler implementations end up being pretty thin and add a lot of overhead.
    /// A pragmatic approach to this would be to just write the data access code directly in the handler, but this violates clean architecture.
    /// One solution that retains clean architecture is to use a prebuilt handler implementation which requires an implementation of a query logic abstraction.
    /// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
    /// </summary>
    /// <param name="eventRaisingFacade"></param>
    /// <param name="queryLogic"></param>
    public SimpleQueryHandler(IEventRaisingFacade eventRaisingFacade, IQueryLogic<TQuery, TResult> queryLogic)
        : base(eventRaisingFacade)
    {
        _queryLogic = queryLogic;
    }

    /// <inheritdoc />
    public override async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        return await _queryLogic.ExecuteAsync(query, cancellationToken);
    }
}
