using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class LoggingQueryHandler<TQuery, TResponse> : IHandler<TQuery, TResponse>
{
    private readonly ILogger<LoggingQueryHandler<TQuery, TResponse>> _logger;

    public LoggingQueryHandler(ILogger<LoggingQueryHandler<TQuery, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse?> HandleAsync(TQuery request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Type queryType = typeof(TQuery);

        using (_logger.BeginScope("Query: {QueryType}", queryType.Name))
        {
            _logger.LogDebug("Executing query.");
            TResponse response = await next();
            _logger.LogDebug("Query was handled successfully.");
            return response;
        }
    }
}
