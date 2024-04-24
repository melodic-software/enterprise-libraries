using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.TEMP.Diagnostics;

public static class TimedOperationExtensions
{
    public static IDisposable BeginTimedOperation(this ILogger logger, string messageTemplate, params object[] args)
    {
        return logger.BeginTimedOperation(LogLevel.Information, messageTemplate, args);
    }

    public static IDisposable BeginTimedOperation(this ILogger logger, LogLevel logLevel, string messageTemplate, params object[] args)
    {
        return new TimedOperation(logger, logLevel, messageTemplate, args);
    }
}