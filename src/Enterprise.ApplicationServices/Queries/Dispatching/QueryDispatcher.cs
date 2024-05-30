using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Dispatching;

public class QueryDispatcher : IDispatchQueries
{
    private readonly IResolveQueryHandler _queryHandlerResolver;

    public QueryDispatcher(IResolveQueryHandler queryHandlerResolver)
    {
        _queryHandlerResolver = queryHandlerResolver;
    }

    /// <inheritdoc />
    public async Task<TResponse> DispatchAsync<TResponse>(IBaseQuery query, CancellationToken cancellationToken)
    {
        IHandleQuery<TResponse> handler = _queryHandlerResolver.GetQueryHandler<TResponse>(query);
        TResponse response = await handler.HandleAsync(query, cancellationToken);
        return response;
    }

    /// <inheritdoc />
    public async Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        IHandleQuery<TResponse> handler = _queryHandlerResolver.GetQueryHandler(query);
        TResponse response = await handler.HandleAsync(query, cancellationToken);
        return response;
    }

    /// <inheritdoc />
    public async Task<TResponse> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IBaseQuery
    {
        IHandleQuery<TQuery, TResponse> handler = _queryHandlerResolver.GetQueryHandler<TQuery, TResponse>(query);
        TResponse response = await handler.HandleAsync(query, cancellationToken);
        return response;
    }
}
