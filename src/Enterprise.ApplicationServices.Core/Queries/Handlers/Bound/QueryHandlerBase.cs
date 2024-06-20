using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;

/// <summary>
/// A base implementation of a query handler that supports the raising of events.
/// The result type is bound to the type defined by the query.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class QueryHandlerBase<TQuery, TResult> :
    ApplicationServiceBase, IHandleQuery<TQuery, TResult>, IHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
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
    async Task<TResult?> IHandler<TQuery, TResult>.HandleAsync(TQuery request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }

    async Task<TResult> Handlers.IHandleQuery<TQuery, TResult>.HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        return await HandleAsync(query, cancellationToken);
    }

    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
