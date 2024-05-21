namespace Enterprise.ApplicationServices.Core.Queries.Dispatching;

public interface IDispatchQueries
{
    Task<TResponse?> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IBaseQuery;
}
