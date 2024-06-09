using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;

/// <summary>
/// Handles queries.
/// The result type is bound to the type defined by the query.
/// </summary>
/// <typeparam name="TQuery">The explicit type of query that can be handled.</typeparam>
/// <typeparam name="TResult">The expected result type.</typeparam>
public interface IHandleQuery<in TQuery, TResult> : Handlers.IHandleQuery<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    /// <summary>
    /// Handle a specific type of query and return the typed result.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    new Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
