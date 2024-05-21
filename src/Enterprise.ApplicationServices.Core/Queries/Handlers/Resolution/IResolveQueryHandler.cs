using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;

/// <summary>
/// Resolves query handler implementations that can handle specific queries.
/// </summary>
public interface IResolveQueryHandler
{
    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(TQuery query) where TQuery : IBaseQuery;
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(IQuery<TResponse> query) where TQuery : IQuery<TResponse>;
    public object GetQueryHandler<TResponse>(IQuery<TResponse> query);
}
