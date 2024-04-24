using Enterprise.Logging.Core.Operations.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.Operations;

public class OperationLogger : IOperationLogger
{
    private readonly ILogger _logger;

    public OperationLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void LogOperationDuration(string messageTemplate, LogLevel logLevel, TimeSpan duration, params object[] args)
    {
        object[] argsWithTiming = args.Append(duration.TotalMilliseconds).ToArray();
        _logger.Log(logLevel, $"{messageTemplate} completed in {{OperationDurationMs}}ms", argsWithTiming);
    }
}