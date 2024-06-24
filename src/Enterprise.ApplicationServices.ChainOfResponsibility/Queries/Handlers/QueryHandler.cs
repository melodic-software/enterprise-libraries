using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;
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
    async Task<object?> IHandleQuery.HandleAsync(IQuery query, CancellationToken cancellationToken)
    {
        return await HandleAsync(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(IQuery query, CancellationToken cancellationToken = default)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResult result = await HandleAsync(typedQuery, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        return await _responsibilityChain.HandleAsync(query, cancellationToken);
    }
}
