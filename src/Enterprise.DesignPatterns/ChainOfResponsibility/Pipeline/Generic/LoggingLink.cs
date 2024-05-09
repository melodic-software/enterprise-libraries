using System.Diagnostics;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Generic;

public class LoggingLink<TRequest, TResponse> : IChainLink<TRequest>, IChainLink<TRequest, TResponse>
{
    private readonly ILogger<LoggingLink<TRequest, TResponse>> _logger;

    public LoggingLink(ILogger<LoggingLink<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteAsync(TRequest request, NextChainLinkDelegate next, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

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

    public async Task<TResponse?> ExecuteAsync(TRequest request, NextChainLinkDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

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
