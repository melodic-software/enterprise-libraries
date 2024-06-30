using System.Diagnostics;
using Enterprise.MediatR.Behaviors.Logging.Services.Abstract;
using Enterprise.MediatR.Behaviors.Logging.Services;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Logging;

public class RequestLoggingBehavior<TRequest, TResult> :
    IPipelineBehavior<TRequest, TResult>
    where TRequest : notnull
{
    private readonly ILogger<RequestLoggingBehavior<TRequest, TResult>> _logger;
    private readonly IRequestLoggingBehaviorService _loggingBehaviorService;

    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResult>> logger) : this(logger, new RequestLoggingBehaviorService())
    {

    }

    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResult>> logger, IRequestLoggingBehaviorService loggingBehaviorService)
    {
        _logger = logger;
        _loggingBehaviorService = loggingBehaviorService;
    }

    public async Task<TResult> Handle(TRequest request,
        RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Type requestType = request.GetType();
        string requestTypeName = requestType.Name;

        var stopWatch = Stopwatch.StartNew();

        using IDisposable scope = _logger.BeginScope("{@Request}", request);

        _logger.LogInformation("Executing request.");
        TResult result = await next();
        HandleResult(result, requestTypeName);

        stopWatch.Stop();

        _logger.LogInformation("Handled \"{Request}\" in {Milliseconds} ms.",
            requestTypeName, stopWatch.ElapsedMilliseconds);

        return result;
    }

    private void HandleResult(TResult genericResult, string requestTypeName)
    {
        if (genericResult is not Result result)
        {
            _logger.LogInformation("Request execution completed.");
            return;
        }

        if (result.IsSuccess)
        {
            _logger.LogInformation("Request was successful.");
        }
        else
        {
            _loggingBehaviorService.LogApplicationServiceError(_logger, result.Errors, requestTypeName);
        }
    }
}
