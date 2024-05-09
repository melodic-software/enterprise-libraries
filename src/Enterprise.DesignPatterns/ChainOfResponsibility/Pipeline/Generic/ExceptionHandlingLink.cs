using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Generic;

public class ExceptionHandlingLink<TRequest, TResponse> : IChainLink<TRequest>, IChainLink<TRequest, TResponse>
{
    private readonly ILogger<ExceptionHandlingLink<TRequest, TResponse>> _logger;

    public ExceptionHandlingLink(ILogger<ExceptionHandlingLink<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteAsync(TRequest request, NextChainLinkDelegate next, CancellationToken cancellationToken)
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred processing request: {Request}.", typeof(TRequest).Name);
            throw;
        }
    }

    public async Task<TResponse?> ExecuteAsync(TRequest request, NextChainLinkDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred processing request: {Request}.", typeof(TRequest).Name);
            throw;
        }
    }
}
