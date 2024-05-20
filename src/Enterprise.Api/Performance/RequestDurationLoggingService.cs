using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Performance;

[Obsolete("This is only for demonstration purposes, and eventually will be removed.")]
internal static class RequestDurationLoggingService
{
    internal static void LogRequestDuration(Stopwatch stopWatch, ILogger logger)
    {
        logger.LogInformation("Request completed in {ElapsedMilliseconds} ms.", stopWatch.ElapsedMilliseconds);
    }
}