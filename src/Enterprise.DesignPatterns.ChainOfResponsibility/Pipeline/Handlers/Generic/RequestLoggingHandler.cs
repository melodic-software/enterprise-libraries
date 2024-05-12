using System.Diagnostics;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.Generic;

/// <summary>
/// Generic handler for logging the processing of requests.
/// </summary>
/// <typeparam name="TRequest">The type of request the handler processes.</typeparam>
/// <typeparam name="TResponse">The type of response the handler returns.</typeparam>
public class RequestLoggingHandler<TRequest, TResponse> : IHandler<TRequest>, IHandler<TRequest, TResponse>
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
    /// Asynchronously handles and logs the request.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next delegate in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    public async Task HandleAsync(TRequest request, SuccessorDelegate next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Starting handling request: {RequestType}.", typeof(TRequest).Name);

        try
        {
            await next();
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
                "Completed handling request: {RequestType} in {ElapsedMilliseconds} ms.",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds
            );
        }
    }

    /// <summary>
    /// Asynchronously handles, logs the request, and returns a response.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next delegate in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    /// <returns>The processed response.</returns>
    public async Task<TResponse?> HandleAsync(TRequest request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Starting handling request: {RequestType}.", typeof(TRequest).Name);

        TResponse response;

        try
        {
            response = await next();
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
                "Completed handling request: {RequestType} in {ElapsedMilliseconds} ms.",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds
            );
        }

        return response;
    }
}
