using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Facade;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Facade;

internal sealed class QueryFacadeService : IQueryFacadeService
{
    private readonly IDispatchQueries _queryDispatcher;

    public QueryFacadeService(IDispatchQueries queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
    }

    /// <inheritdoc />
    public async Task<TResponse?> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IBaseQuery
    {
        return await _queryDispatcher.DispatchAsync<TQuery, TResponse>(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        return await _queryDispatcher.DispatchAsync(query, cancellationToken);
    }
}
