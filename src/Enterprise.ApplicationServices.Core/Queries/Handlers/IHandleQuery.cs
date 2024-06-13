using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

/// <summary>
/// Handles queries.
/// </summary>
/// <typeparam name="TQuery">The explicit type of query that can be handled.</typeparam>
/// <typeparam name="TResult">The expected result type.</typeparam>
public interface IHandleQuery<in TQuery, TResult> : IHandleQuery<TResult> 
    where TQuery : class, IQuery
{
    /// <summary>
    /// Handle a specific type of query and return the typed result.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
