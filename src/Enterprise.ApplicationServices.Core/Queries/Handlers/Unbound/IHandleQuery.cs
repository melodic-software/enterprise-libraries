using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Standard;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;

/// <summary>
/// Handles queries.
/// The type of response is not directly associated with the query passed in.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IHandleQuery<TResponse> : IApplicationService
{
    /// <summary>
    /// Handle the query and return the typed result.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> HandleAsync(IQuery query, CancellationToken cancellationToken);
}
