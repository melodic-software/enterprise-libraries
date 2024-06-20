﻿using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Bound.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers.Bound;

public class LoggingQueryHandler<TQuery, TResult> : QueryHandlerDecoratorBase<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly ILogger<LoggingQueryHandler<TQuery, TResult>> _logger;

    public LoggingQueryHandler(IHandleQuery<TQuery, TResult> queryHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingQueryHandler<TQuery, TResult>> logger) : base(queryHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        Type queryType = typeof(TQuery);
        Type innermostHandlerType = Innermost.GetType();

        // TODO: Do we want to add a scope (or log statement) that describes the decorator chain?
        // Maybe we do that in the base?

        using (_logger.BeginScope("Query Handler: {QueryHandlerType}, Query: {QueryType}", innermostHandlerType.Name, queryType.Name))
        {
            _logger.LogDebug("Executing query.");
            TResult result = await Decorated.HandleAsync(query, cancellationToken);
            _logger.LogDebug("Query was handled successfully.");
            return result;
        }
    }
}