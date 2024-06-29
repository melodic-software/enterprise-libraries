﻿using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Model.Base;
using Enterprise.ApplicationServices.Core.Queries.Model.Generic;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers.Bound.Abstract;

public abstract class QueryHandlerDecoratorBase<TQuery, TResult> : 
    DecoratorBase<IHandleQuery<TQuery, TResult>>,
    IHandleQuery<TQuery, TResult> where TQuery : class, IQuery<TResult>
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
    async Task<TResult> Core.Queries.Handlers.IHandleQuery<TQuery, TResult>.HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        return await HandleAsync(query, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
