using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class ErrorHandlingQueryHandler<TQuery, TResult> : IHandler<TQuery, TResult>
{
    private readonly ILogger<ErrorHandlingQueryHandler<TQuery, TResult>> _logger;

    public ErrorHandlingQueryHandler(ILogger<ErrorHandlingQueryHandler<TQuery, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult?> HandleAsync(TQuery request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken = default)
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
