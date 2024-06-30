using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model.Generic;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;

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
    public IHandleQuery<TResult> GetHandlerFor<TResult>(IQuery query);

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TResult> GetHandlerFor<TResult>(IQuery<TResult> query);

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TQuery, TResult> GetHandlerFor<TQuery, TResult>(TQuery query)
        where TQuery : class, IQuery;

    /// <summary>
    /// Get the handler implementation that can handle the given query.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public IHandleQuery<TQuery, TResult> GetHandlerFor<TQuery, TResult>(IQuery<TResult> query)
        where TQuery : class, IQuery<TResult>;
}
