using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public sealed class QueryHandler<TQuery, TResult> : IHandleQuery<TQuery, TResult>
    where TQuery : class, IQuery
{
    private readonly IResponsibilityChain<TQuery, TResult> _responsibilityChain;

    public QueryHandler(IResponsibilityChain<TQuery, TResult> responsibilityChain)
    {
        _responsibilityChain = responsibilityChain;
    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(IBaseQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResult result = await HandleAsync(typedQuery, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        return await _responsibilityChain.HandleAsync(query, cancellationToken);
    }
}
