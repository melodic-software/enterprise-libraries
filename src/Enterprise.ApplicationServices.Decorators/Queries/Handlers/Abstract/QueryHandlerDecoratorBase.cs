using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;

public abstract class QueryHandlerDecoratorBase<TQuery, TResult> : 
    DecoratorBase<IHandleQuery<TQuery, TResult>>,
    IHandleQuery<TQuery, TResult> where TQuery : IBaseQuery
{
    protected QueryHandlerDecoratorBase(IHandleQuery<TQuery, TResult> queryHandler, IGetDecoratedInstance decoratorService)
        : base(queryHandler, decoratorService)
    {

    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(IBaseQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResult response = await HandleAsync(typedQuery, cancellationToken);
        return response;
    }

    /// <inheritdoc />
    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
