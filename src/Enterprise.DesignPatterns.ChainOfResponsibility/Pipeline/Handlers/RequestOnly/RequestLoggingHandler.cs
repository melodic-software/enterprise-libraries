using System.Diagnostics;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly;

/// <summary>
/// Generic handler for logging the processing of requests.
/// </summary>
/// <typeparam name="TRequest">The type of request the handler processes.</typeparam>
public class RequestLoggingHandler<TRequest> : IHandler<TRequest>
{
    private readonly ILogger<RequestLoggingHandler<TRequest>> _logger;

    /// <summary>
    /// Initializes a new instance of the RequestLoggingHandler class.
    /// </summary>
    /// <param name="logger">Logger for recording request processing information.</param>
    public RequestLoggingHandler(ILogger<RequestLoggingHandler<TRequest>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously handles and logs the request.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next delegate in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    public async Task HandleAsync(TRequest request, SuccessorDelegate next, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        using IDisposable scope = _logger.BeginScope("{@Request}", request);

        _logger.LogInformation("Handling request.");

        await next();

        stopwatch.Stop();

        _logger.LogInformation(
            "Request handling complete in {ElapsedMilliseconds} ms.",
            stopwatch.ElapsedMilliseconds
        );
    }
}
