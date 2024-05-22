using Enterprise.ApplicationServices.Core.Queries.Model;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

public abstract class QueryHandlerBase<TQuery, TResponse> : IHandleQuery<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(IBaseQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResponse result = await HandleAsync(typedQuery, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public abstract Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
