using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.ApplicationServices.Queries.Dispatching;

public class QueryDispatcher : IDispatchQueries
{
    private readonly IResolveQueryHandler _queryHandlerResolver;

    public QueryDispatcher(IResolveQueryHandler queryHandlerResolver)
    {
        _queryHandlerResolver = queryHandlerResolver;
    }

    /// <inheritdoc />
    public async Task<Result<TResponse>> DispatchAsync<TResponse>(IQuery query, CancellationToken cancellationToken)
    {
        IHandleQuery<TResponse> queryHandler = _queryHandlerResolver.GetQueryHandler<TResponse>(query);
        Result<TResponse> result = await queryHandler.HandleAsync(query, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<Result<TResponse>> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        IHandleQuery<TResponse> queryHandler = _queryHandlerResolver.GetQueryHandler(query);
        Result<TResponse> result = await queryHandler.HandleAsync(query, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<Result<TResponse>> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery
    {
        IHandleQuery<TQuery, TResponse> queryHandler = _queryHandlerResolver.GetQueryHandler<TQuery, TResponse>(query);
        Result<TResponse> result = await queryHandler.HandleAsync(query, cancellationToken);
        return result;
    }
}
