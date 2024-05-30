using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers.Simple;

/// <summary>
/// Most query handler implementations end up being pretty thin...
/// Some pragmatic approaches involve writing the data access code directly in the handler, but this violates clean architecture.
/// One solution is to use a prebuilt handler implementation which requires a query logic implementation.
/// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class SimpleQueryHandler<TQuery, TResponse> :
    QueryHandlerBase<TQuery, TResponse> 
    where TQuery : IBaseQuery
{
    private readonly IQueryLogic<TQuery, TResponse> _queryLogic;

    /// <summary>
    /// Most query handler implementations end up being pretty thin and add a lot of overhead.
    /// A pragmatic approach to this would be to just write the data access code directly in the handler, but this violates clean architecture.
    /// One solution that retains clean architecture is to use a prebuilt handler implementation which requires an implementation of a query logic abstraction.
    /// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
    /// </summary>
    /// <param name="eventRaisingFacade"></param>
    /// <param name="queryLogic"></param>
    public SimpleQueryHandler(IEventRaisingFacade eventRaisingFacade, IQueryLogic<TQuery, TResponse> queryLogic)
        : base(eventRaisingFacade)
    {
        _queryLogic = queryLogic;
    }

    /// <inheritdoc />
    public override async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        return await _queryLogic.ExecuteAsync(query, cancellationToken);
    }
}
