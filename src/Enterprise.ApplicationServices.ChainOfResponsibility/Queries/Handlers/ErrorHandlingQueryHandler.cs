using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class ErrorHandlingQueryHandler<TQuery, TResponse> : IHandler<TQuery, TResponse>
{
    private readonly ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> _logger;

    public ErrorHandlingQueryHandler(ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse?> HandleAsync(TQuery request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            Type queryType = typeof(TQuery);
            _logger.LogError(exception, "An error occurred while handling the \"{QueryType}\" query.", queryType.Name);
            throw;
        }
    }
}
