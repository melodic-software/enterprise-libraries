using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Decorators.QueryHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.ApplicationServices.Decorators.QueryHandlers;

public class NullQueryValidationQueryHandler<TQuery, TResponse> : QueryHandlerDecoratorBase<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    public NullQueryValidationQueryHandler(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService) : base(queryHandler, decoratorService)
    {

    }

    public override Task<TResponse> HandleAsync(TQuery? query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return Decorated.HandleAsync(query, cancellationToken);
    }
}