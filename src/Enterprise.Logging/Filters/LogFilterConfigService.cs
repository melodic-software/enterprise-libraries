using Enterprise.Logging.Core.Filters;
using Enterprise.Logging.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Filters;

internal static class LogFilterConfigService
{
    internal static void ConfigureFilters(this IHostApplicationBuilder builder, LoggingConfigOptions configOptions)
    {
        ConfigureProduction(builder);

        // Allow for manual registration of log filters.
        // This is an alternate over the appSettings.config file configuration.
        configOptions.AddLogFilters?.Invoke(builder.Logging);
    }

    private static void ConfigureProduction(IHostApplicationBuilder builder)
    {
        if (!builder.Environment.IsProduction())
        {
            return;
        }

        // Ensure that trace logging can never be enabled in production.
        // These messages may contain sensitive application data.
        builder.Logging.AddFilter(LogFilters.MinimumLogLevel(LogLevel.Debug));
    }
}
