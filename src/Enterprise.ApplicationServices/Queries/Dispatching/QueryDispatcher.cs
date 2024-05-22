using System.Reflection;
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
    public async Task<TResponse?> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IBaseQuery
    {
        IHandleQuery<TQuery, TResponse> queryHandler = _queryHandlerResolver.GetQueryHandler<TQuery, TResponse>(query);
        TResponse result = await queryHandler.HandleAsync(query, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<TResponse?> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        object queryHandler = _queryHandlerResolver.GetQueryHandler(query);

        Type queryHandlerType = queryHandler.GetType();
        MethodInfo? method = queryHandlerType.GetMethod(nameof(IHandleQuery<IQuery<TResponse>, TResponse>.HandleAsync));

        TResponse? result = await (Task<TResponse?>)
        (
            method?.Invoke(queryHandler, [query]) ??
            throw new InvalidOperationException("Query handling task is invalid.")
        );
     
        return result;
    }
}
