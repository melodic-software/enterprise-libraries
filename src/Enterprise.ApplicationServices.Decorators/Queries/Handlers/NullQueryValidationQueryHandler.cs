using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

public class NullQueryValidationQueryHandler<TQuery, TResponse> : QueryHandlerDecoratorBase<TQuery, TResponse>
    where TQuery : IQuery
{
    public NullQueryValidationQueryHandler(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService) : base(queryHandler, decoratorService)
    {

    }

    public override Task<Result<TResponse>> HandleAsync(TQuery? query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return Decorated.HandleAsync(query, cancellationToken);
    }
}
