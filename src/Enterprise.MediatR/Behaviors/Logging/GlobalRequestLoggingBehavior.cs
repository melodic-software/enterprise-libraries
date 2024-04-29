using System.Diagnostics;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Logging;

public class GlobalRequestLoggingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<GlobalRequestLoggingBehavior<TRequest, TResponse>> _logger;

    public GlobalRequestLoggingBehavior(ILogger<GlobalRequestLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Type requestType = request.GetType();

        LogProperties(request, requestType);

        Stopwatch stopWatch = Stopwatch.StartNew();

        TResponse response = await next();

        _logger.LogInformation("Handled {RequestName}, with {Response} in {Milliseconds} ms", 
            requestType.Name, typeof(TResponse).Name, stopWatch.ElapsedMilliseconds);

        stopWatch.Stop();

        return response;
    }

    private void LogProperties(TRequest request, Type requestType)
    {
        if (!_logger.IsEnabled(LogLevel.Debug))
            return;

        _logger.LogDebug("Handling {RequestName}.", requestType.Name);

        // This is using reflection! It could be a performance concern.
        // This may not be something we want to enable in production.
        // We can restrict by environment, OR by log level.
        // For now, we're just going to default to only run if the debugging log level is enabled.
        IList<PropertyInfo> props = new List<PropertyInfo>(requestType.GetProperties());

        foreach (PropertyInfo prop in props)
        {
            object? propValue = prop.GetValue(request, null);
            _logger.LogDebug("Property {Property} : {@Value}", prop?.Name, propValue);
        }
    }
}
