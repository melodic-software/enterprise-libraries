using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.Events.Facade.Abstract;

namespace Enterprise.ApplicationServices.QueryHandlers;

/// <summary>
/// Most query handler implementations end up being pretty thin...
/// Some pragmatic approaches involve writing the data access code directly in the handler, but this violates clean architecture.
/// One solution is to use a prebuilt handler implementation which requires a query logic implementation.
/// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class SimpleQueryHandler<TQuery, TResponse> : QueryHandlerBase<TQuery, TResponse> where TQuery : IBaseQuery
{
    private readonly IQueryLogic<TQuery, TResponse> _queryLogic;

    /// <summary>
    /// Most query handler implementations end up being pretty thin and add a lot of overhead.
    /// A pragmatic approach to this would be to just write the data access code directly in the handler, but this violates clean architecture.
    /// One solution that retains clean architecture is to use a prebuilt handler implementation which requires an implementation of a query logic abstraction.
    /// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
    /// </summary>
    /// <param name="eventServiceFacade"></param>
    /// <param name="queryLogic"></param>
    public SimpleQueryHandler(IEventServiceFacade eventServiceFacade, IQueryLogic<TQuery, TResponse> queryLogic) : base(eventServiceFacade)
    {
        _queryLogic = queryLogic;
    }

    /// <inheritdoc />
    public override async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        return await _queryLogic.ExecuteAsync(query, cancellationToken);
    }
}
