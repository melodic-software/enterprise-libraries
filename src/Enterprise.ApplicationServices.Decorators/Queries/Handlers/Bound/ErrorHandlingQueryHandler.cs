﻿using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Bound.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers.Bound;

public class ErrorHandlingQueryHandler<TQuery, TResult> : QueryHandlerDecoratorBase<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly ILogger<ErrorHandlingQueryHandler<TQuery, TResult>> _logger;

    public ErrorHandlingQueryHandler(IHandleQuery<TQuery, TResult> queryHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingQueryHandler<TQuery, TResult>> logger) : base(queryHandler, decoratorService)
    {
        _logger = logger;
    }

    public override Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            return Decorated.HandleAsync(query, cancellationToken);
        }
        catch (Exception exception)
        {
            Type queryType = typeof(TQuery);
            _logger.LogError(exception, "An error occurred while handling the \"{QueryType}\" query.", queryType.Name);
            throw;
        }
    }
}