using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

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
        Type innermostHandlerType = Innermost.GetType();

        // TODO: Do we want to add a scope (or log statement) that describes the decorator chain?
        // Maybe we do that in the base?

        using (_logger.BeginScope("Query Handler: {QueryHandlerType}, Query: {QueryType}", innermostHandlerType.Name, queryType.Name))
        {
            _logger.LogDebug("Executing query.");
            TResponse response = await Decorated.HandleAsync(query, cancellationToken);
            _logger.LogDebug("Query was handled successfully.");
            return response;
        }
    }
}
