using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;

/// <summary>
/// Handles queries.
/// The type of result is not directly associated with the query passed in.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IHandleQuery<TResult> : IHandleQuery
{
    /// <summary>
    /// Handle the query and return the typed result.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    new Task<TResult> HandleAsync(IQuery query, CancellationToken cancellationToken = default);
}
