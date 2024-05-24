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
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TResponse> GetQueryHandler<TResponse>(IQuery query);

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TResponse> GetQueryHandler<TResponse>(IQuery<TResponse> query);

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(TQuery query) where TQuery : IQuery;

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(IQuery<TResponse> query) where TQuery : IQuery<TResponse>;
}
