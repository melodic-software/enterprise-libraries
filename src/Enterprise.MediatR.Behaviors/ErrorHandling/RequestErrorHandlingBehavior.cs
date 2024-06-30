using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.ErrorHandling;

public class RequestErrorHandlingBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult> where TRequest : IBaseRequest 
{
    private readonly ILogger<RequestErrorHandlingBehavior<TRequest, TResult>> _logger;

    public RequestErrorHandlingBehavior(ILogger<RequestErrorHandlingBehavior<TRequest, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing the request.");
            throw;
        }
    }
}
