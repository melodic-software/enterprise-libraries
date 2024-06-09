using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;

/// <summary>
/// Resolves query handler implementations that can handle specific queries.
/// </summary>
public interface IResolveQueryHandler
{
    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TResult> GetQueryHandler<TResult>(IQuery query);

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TResult> GetQueryHandler<TResult>(IQuery<TResult> query);

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(TQuery query)
        where TQuery : class, IQuery;

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(IQuery<TResult> query)
        where TQuery : class, IQuery<TResult>;
}
