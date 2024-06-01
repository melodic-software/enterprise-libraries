using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public sealed class QueryHandler<TQuery, TResponse> : IHandleQuery<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    private readonly IResponsibilityChain<TQuery, TResponse> _responsibilityChain;

    public QueryHandler(IResponsibilityChain<TQuery, TResponse> responsibilityChain)
    {
        _responsibilityChain = responsibilityChain;
    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(IBaseQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResponse response = await HandleAsync(typedQuery, cancellationToken);
        return response;
    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        return await _responsibilityChain.HandleAsync(query, cancellationToken);
    }
}
