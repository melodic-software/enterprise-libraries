using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class LoggingQueryHandler<TQuery, TResult> : IHandler<TQuery, TResult>
{
    private readonly ILogger<LoggingQueryHandler<TQuery, TResult>> _logger;

    public LoggingQueryHandler(ILogger<LoggingQueryHandler<TQuery, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult?> HandleAsync(TQuery request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken)
    {
        Type queryType = typeof(TQuery);

        using (_logger.BeginScope("Query: {QueryType}", queryType.Name))
        {
            _logger.LogDebug("Executing query.");
            TResult response = await next();
            _logger.LogDebug("Query was handled successfully.");
            return response;
        }
    }
}
