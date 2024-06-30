using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationException = Enterprise.Exceptions.Model.ApplicationException;

namespace Enterprise.MediatR.Behaviors.ErrorHandling;

public class RequestExceptionHandlingBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult> where TRequest : class 
{
    private readonly ILogger<RequestExceptionHandlingBehavior<TRequest, TResult>> _logger;

    public RequestExceptionHandlingBehavior(ILogger<RequestExceptionHandlingBehavior<TRequest, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(
        TRequest request,
        RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while executing the request.");
            throw new ApplicationException(typeof(TRequest).Name, error: null, innerException: ex);
        }
    }
}
