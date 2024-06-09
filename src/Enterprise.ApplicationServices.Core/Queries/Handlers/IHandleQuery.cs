﻿using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Standard;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

public interface IHandleQuery<TResponse> : IApplicationService
{
    /// <summary>
    /// Handle the query and return the typed result.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> HandleAsync(IBaseQuery query, CancellationToken cancellationToken);
}

/// <summary>
/// Handles queries.
/// </summary>
/// <typeparam name="TQuery">The explicit type of query that can be handled.</typeparam>
/// <typeparam name="TResponse">The expected result type.</typeparam>
public interface IHandleQuery<in TQuery, TResponse> : IHandleQuery<TResponse> where TQuery : class, IBaseQuery
{
    /// <summary>
    /// Handle a specific type of query and return the typed result.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
