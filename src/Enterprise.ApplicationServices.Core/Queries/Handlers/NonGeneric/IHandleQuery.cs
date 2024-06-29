using Enterprise.ApplicationServices.Core.Queries.Model.Base;
using Enterprise.ApplicationServices.Core.Standard;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;

/// <summary>
/// Handles queries.
/// </summary>
public interface IHandleQuery : IApplicationService
{
    /// <summary>
    /// Handle the query and return a result object.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<object?> HandleAsync(IBaseQuery query, CancellationToken cancellationToken = default);
}
