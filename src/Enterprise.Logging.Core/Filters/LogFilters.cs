using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.Filters;

public static class LogFilters
{
    public static Func<string?, string?, LogLevel, bool> MinimumLogLevel(LogLevel minimumLevel) => 
        (provider, category, logLevel) => 
            logLevel >= minimumLevel;
}