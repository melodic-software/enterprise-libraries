using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;

public abstract class QueryHandlerDecoratorBase<TQuery, TResponse> : DecoratorBase<IHandleQuery<TQuery, TResponse>>,
    IHandleQuery<TQuery, TResponse> where TQuery : IQuery
{
    protected QueryHandlerDecoratorBase(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService)
        : base(queryHandler, decoratorService)
    {

    }

    /// <inheritdoc />
    public async Task<Result<TResponse>> HandleAsync(IQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        Result<TResponse> result = await HandleAsync(typedQuery, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public abstract Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
