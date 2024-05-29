﻿using Enterprise.ApplicationServices.Core.Queries.Dispatching;
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
        IHandleQuery<TResponse> handler = _queryHandlerResolver.GetQueryHandler<TResponse>(query);

        if (handler == null)
        {
            throw new InvalidOperationException(
                $"An implementation of {nameof(IHandleQuery<TResponse>)} " +
                $"could not be resolved for query \"{query.GetType().FullName}\" " +
                $"with \"{typeof(TResponse)}\" response."
            );
        }

        Result<TResponse> result = await handler.HandleAsync(query, cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<Result<TResponse>> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        IHandleQuery<TResponse> handler = _queryHandlerResolver.GetQueryHandler(query);

        if (handler == null)
        {
            throw new InvalidOperationException(
                $"An implementation of {nameof(IHandleQuery<TResponse>)} " +
                $"could not be resolved for query \"{query.GetType().FullName}\" " +
                $"with \"{typeof(TResponse)}\" response."
            );
        }

        Result<TResponse> result = await handler.HandleAsync(query, cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<Result<TResponse>> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery
    {
        IHandleQuery<TQuery, TResponse> handler = _queryHandlerResolver.GetQueryHandler<TQuery, TResponse>(query);

        if (handler == null)
        {
            throw new InvalidOperationException(
                $"An implementation of {nameof(IHandleQuery<TQuery, TResponse>)} " +
                $"could not be resolved for query \"{typeof(TQuery).FullName}\" " +
                $"with \"{typeof(TResponse)}\" response."
            );
        }

        Result<TResponse> result = await handler.HandleAsync(query, cancellationToken);

        return result;
    }
}
