﻿using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

public class NullQueryValidationQueryHandler<TQuery, TResult> : QueryHandlerDecoratorBase<TQuery, TResult>
    where TQuery : class, IQuery
{
    public NullQueryValidationQueryHandler(IHandleQuery<TQuery, TResult> queryHandler,
        IGetDecoratedInstance decoratorService) : base(queryHandler, decoratorService)
    {

    }

    public override Task<TResult> HandleAsync(TQuery? query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return Decorated.HandleAsync(query, cancellationToken);
    }
}
