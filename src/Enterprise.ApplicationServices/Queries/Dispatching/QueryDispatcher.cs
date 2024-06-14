﻿using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

namespace Enterprise.ApplicationServices.Queries.Dispatching;

public class QueryDispatcher : IDispatchQueries
{
    private readonly IResolveQueryHandler _queryHandlerResolver;

    public QueryDispatcher(IResolveQueryHandler queryHandlerResolver)
    {
        _queryHandlerResolver = queryHandlerResolver;
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TResult>(IQuery query, CancellationToken cancellationToken = default)
    {
        IHandleQuery<TResult> handler = _queryHandlerResolver.GetQueryHandler<TResult>(query);
        TResult result = await handler.HandleAsync(query, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        IHandleQuery<TResult> handler = _queryHandlerResolver.GetQueryHandler(query);
        TResult result = await handler.HandleAsync(query, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class, IQuery
    {
        IHandleQuery<TQuery, TResult> handler = _queryHandlerResolver.GetQueryHandler<TQuery, TResult>(query);
        TResult result = await handler.HandleAsync(query, cancellationToken);
        return result;
    }
}
