using System.Diagnostics;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Logging;

public class RequestLoggingBehavior<TRequest, TResult> :
    IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly ILogger<RequestLoggingBehavior<TRequest, TResult>> _logger;

    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(TRequest request,
        RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Type requestType = request.GetType();

        LogProperties(request, requestType);

        var stopWatch = Stopwatch.StartNew();

        TResult result = await next();

        _logger.LogInformation("Handled {RequestName}, with {Result} in {Milliseconds} ms", 
            requestType.Name, typeof(TResult).Name, stopWatch.ElapsedMilliseconds);

        stopWatch.Stop();

        return result;
    }

    private void LogProperties(TRequest request, Type requestType)
    {
        if (!_logger.IsEnabled(LogLevel.Debug))
        {
            return;
        }

        _logger.LogDebug("Handling {RequestName}.", requestType.Name);

        // This is using reflection! It could be a performance concern.
        // This may not be something we want to enable in production.
        // We can restrict by environment, OR by log level.
        // For now, we're just going to default to only run if the debugging log level is enabled.
        IList<PropertyInfo> props = new List<PropertyInfo>(requestType.GetProperties());

        foreach (PropertyInfo prop in props)
        {
            object? propValue = prop.GetValue(request, null);
            _logger.LogDebug("Property {Property} : {@Value}", prop.Name, propValue);
        }
    }
}
