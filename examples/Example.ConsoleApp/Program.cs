using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;

PreStartupLogger.Instance.LogTrace("This is a test message.");
PreStartupLogger.Instance.LogDebug("This is a test message.");
PreStartupLogger.Instance.LogInformation("This is an test message.");
PreStartupLogger.Instance.LogWarning("This is a test message.");
PreStartupLogger.Instance.LogError("This is an test message.");
PreStartupLogger.Instance.LogCritical("This is a test message.");

try
{
    throw new InvalidOperationException("This is an invalid operation.");
}
catch (Exception ex)
{
    PreStartupLogger.Instance.LogError(ex, ex.Message);
}

Console.ReadKey(true);
