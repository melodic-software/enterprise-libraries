using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse;

/// <summary>
/// Generic handler for managing exceptions during the processing of requests.
/// </summary>
/// <typeparam name="TRequest">The type of request the handler processes.</typeparam>
/// <typeparam name="TResponse">The type of response the handler returns.</typeparam>
public class RequestExceptionHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    private readonly ILogger<RequestExceptionHandler<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the RequestExceptionHandler class.
    /// </summary>
    /// <param name="logger">Logger for recording exception information.</param>
    public RequestExceptionHandler(ILogger<RequestExceptionHandler<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously handles the request and manages exceptions, with response handling.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next delegate in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    /// <returns>The processed response or null in case of an exception.</returns>
    public async Task<TResponse?> HandleAsync(TRequest request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            throw;
        }
    }
}
