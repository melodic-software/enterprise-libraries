using System.Reflection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Enterprise.Serilog.Inspection;

public class ConfiguredSinkLogger
{
    private const string SinkPrivateFieldName = "_sink";
    private const string SinksPrivateFieldName = "_sinks";

    public static void LogConfiguredSinks(LoggerConfiguration loggerConfig, ILogger logger)
    {
        try
        {
            FieldInfo? sinkField = typeof(LoggerConfiguration)
                .GetField(SinkPrivateFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (sinkField?.GetValue(loggerConfig) is not ILogEventSink aggregateSink)
                return;

            FieldInfo? sinksField = aggregateSink.GetType()
                .GetField(SinksPrivateFieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (sinksField?.GetValue(aggregateSink) is not IEnumerable<ILogEventSink> sinks)
                return;

            logger.LogInformation("Configured Sinks:");

            foreach (ILogEventSink sink in sinks)
            {
                Type sinkType = sink.GetType();
                logger.LogInformation(sinkType.Name);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred inspecting the configured Serilog sinks.");
        }
    }
}
