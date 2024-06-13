using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Standard;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;

/// <summary>
/// Handles queries.
/// The type of result is not directly associated with the query passed in.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IHandleQuery<TResult> : IApplicationService
{
    /// <summary>
    /// Handle the query and return the typed result.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult> HandleAsync(IQuery query, CancellationToken cancellationToken = default);
}
