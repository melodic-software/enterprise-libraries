using System.Diagnostics;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse;

/// <summary>
/// Generic handler for logging the processing of requests.
/// </summary>
/// <typeparam name="TRequest">The type of request the handler processes.</typeparam>
/// <typeparam name="TResponse">The type of response the handler returns.</typeparam>
public class RequestLoggingHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    private readonly ILogger<RequestLoggingHandler<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the RequestLoggingHandler class.
    /// </summary>
    /// <param name="logger">Logger for recording request processing information.</param>
    public RequestLoggingHandler(ILogger<RequestLoggingHandler<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously handles, logs the request, and returns a response.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next delegate in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    /// <returns>The processed response.</returns>
    public async Task<TResponse?> HandleAsync(TRequest request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        using IDisposable scope = _logger.BeginScope("{@Request}", request);

        _logger.LogInformation("Handling request.");

        TResponse response = await next();

        stopwatch.Stop();

        _logger.LogInformation(
            "Request handling complete in {ElapsedMilliseconds} ms.",
            stopwatch.ElapsedMilliseconds
        );

        return response;
    }
}
