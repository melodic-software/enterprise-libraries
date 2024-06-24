using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly;

/// <summary>
/// Generic handler for managing exceptions during the processing of requests.
/// </summary>
/// <typeparam name="TRequest">The type of request the handler processes.</typeparam>
public class RequestExceptionHandler<TRequest> : IHandler<TRequest>
{
    private readonly ILogger<RequestExceptionHandler<TRequest>> _logger;

    /// <summary>
    /// Initializes a new instance of the RequestExceptionHandler class.
    /// </summary>
    /// <param name="logger">Logger for recording exception information.</param>
    public RequestExceptionHandler(ILogger<RequestExceptionHandler<TRequest>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously handles the request and manages exceptions.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next delegate in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    public async Task HandleAsync(TRequest request, SuccessorDelegate next, CancellationToken cancellationToken = default)
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            throw;
        }
    }
}
