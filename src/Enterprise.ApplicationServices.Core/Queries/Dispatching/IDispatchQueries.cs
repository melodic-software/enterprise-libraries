﻿using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

namespace Enterprise.ApplicationServices.Core.Queries.Dispatching;

public interface IDispatchQueries
{
    Task<TResult> DispatchAsync<TResult>(IQuery query, CancellationToken cancellationToken);
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : class, IQuery;
}
