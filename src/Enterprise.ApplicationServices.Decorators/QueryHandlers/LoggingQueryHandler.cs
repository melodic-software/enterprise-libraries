﻿using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Decorators.QueryHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.QueryHandlers;

public class LoggingQueryHandler<TQuery, TResponse> : QueryHandlerDecoratorBase<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    private readonly ILogger<LoggingQueryHandler<TQuery, TResponse>> _logger;

    public LoggingQueryHandler(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingQueryHandler<TQuery, TResponse>> logger) : base(queryHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        Type queryType = typeof(TQuery);
        Type innermostHandlerType = InnermostHandler.GetType();

        // TODO: Do we want to add a scope (or log statement) that describes the decorator chain?
        // Maybe we do that in the base?

        using (_logger.BeginScope("Query Handler: {QueryHandlerType}: Query: {QueryType}", innermostHandlerType.Name, queryType.Name))
        {
            _logger.LogDebug("Executing query.");
            TResponse result = await Decorated.HandleAsync(query, cancellationToken);
            _logger.LogDebug("Query was handled successfully.");
            return result;
        }
    }
}