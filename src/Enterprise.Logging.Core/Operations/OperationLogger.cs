using Enterprise.Logging.Core.Operations.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.Operations;

public class OperationLogger : IOperationLogger
{
    private readonly ILogger _logger;

    private const string DurationMessageTemplate = "{MessageTemplate} completed in {OperationDurationMs} ms.";

    public OperationLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void LogOperationDuration(string messageTemplate, LogLevel logLevel, TimeSpan duration, params object[] args)
    {
        object[] argsWithTiming = new object[args.Length + 2];
        argsWithTiming[0] = messageTemplate;
        Array.Copy(args, 0, argsWithTiming, 1, args.Length);
        argsWithTiming[^1] = duration.TotalMilliseconds;

        _logger.Log(logLevel, DurationMessageTemplate, argsWithTiming);
    }
}
