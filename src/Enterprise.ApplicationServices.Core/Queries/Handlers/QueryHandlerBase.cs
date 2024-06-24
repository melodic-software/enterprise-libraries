using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

/// <summary>
/// A base implementation of a query handler that supports the raising of events.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class QueryHandlerBase<TQuery, TResult> : 
    ApplicationServiceBase, IHandleQuery<TQuery, TResult>, IHandler<TQuery, TResult>
    where TQuery : class, IQuery
{
    protected QueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {

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
    public async Task<TResult?> HandleAsync(TQuery request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        return await HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
