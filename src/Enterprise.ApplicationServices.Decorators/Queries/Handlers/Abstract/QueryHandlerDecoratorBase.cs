using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Model.Base;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;

public abstract class QueryHandlerDecoratorBase<TQuery, TResult> : 
    DecoratorBase<IHandleQuery<TQuery, TResult>>,
    IHandleQuery<TQuery, TResult> where TQuery : class, IBaseQuery
{
    protected QueryHandlerDecoratorBase(IHandleQuery<TQuery, TResult> queryHandler, IGetDecoratedInstance decoratorService)
        : base(queryHandler, decoratorService)
    {

    }

    /// <inheritdoc />
    async Task<object?> IHandleQuery.HandleAsync(IBaseQuery query, CancellationToken cancellationToken)
    {
        return await HandleAsync(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(IBaseQuery query, CancellationToken cancellationToken = default)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResult result = await HandleAsync(typedQuery, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
