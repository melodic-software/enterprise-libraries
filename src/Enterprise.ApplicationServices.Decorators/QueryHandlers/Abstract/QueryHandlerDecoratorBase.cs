using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.QueryHandlers.Abstract;

public abstract class QueryHandlerDecoratorBase<TQuery, TResponse> : DecoratorBase<IHandleQuery<TQuery, TResponse>>,
    IHandleQuery<TQuery, TResponse> where TQuery : IBaseQuery
{
    protected QueryHandlerDecoratorBase(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService)
        : base(queryHandler, decoratorService)
    {

    }

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
