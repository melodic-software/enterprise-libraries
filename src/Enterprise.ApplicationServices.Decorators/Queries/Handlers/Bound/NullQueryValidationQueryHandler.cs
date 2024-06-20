using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Bound.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers.Bound;

public class NullQueryValidationQueryHandler<TQuery, TResult> : QueryHandlerDecoratorBase<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    public NullQueryValidationQueryHandler(IHandleQuery<TQuery, TResult> queryHandler,
        IGetDecoratedInstance decoratorService) : base(queryHandler, decoratorService)
    {

    }

    public override Task<TResult> HandleAsync(TQuery? query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        return Decorated.HandleAsync(query, cancellationToken);
    }
}
