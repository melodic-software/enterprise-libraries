using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Dispatching;

public interface IDispatchQueries
{
    Task<Result<TResponse>> DispatchAsync<TResponse>(IBaseQuery query, CancellationToken cancellationToken);
    Task<Result<TResponse>> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);
    Task<Result<TResponse>> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IBaseQuery;
}
