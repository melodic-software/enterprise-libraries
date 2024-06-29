using Enterprise.ApplicationServices.Core.Queries.Model.Generic;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;

namespace Enterprise.ApplicationServices.Core.Queries.Dispatching;

public interface IDispatchQueries
{
    Task<TResult> DispatchAsync<TResult>(IQuery query, CancellationToken cancellationToken = default);
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : class, IQuery;
}
