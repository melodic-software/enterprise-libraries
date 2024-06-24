using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

public class LoggingQueryHandler<TQuery, TResult> : QueryHandlerDecoratorBase<TQuery, TResult>
    where TQuery : class, IQuery
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
        using (_logger.BeginScope("Query Handler: {@QueryHandler}, Query: {@Query}", Innermost, query))
        {
            _logger.LogDebug("Executing query.");
            TResult result = await Decorated.HandleAsync(query, cancellationToken);
            _logger.LogDebug("Query was handled successfully.");
            return result;
        }
    }
}
