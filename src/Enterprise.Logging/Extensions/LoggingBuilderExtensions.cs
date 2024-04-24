using Enterprise.Logging.Providers;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Extensions;

public static class LoggingBuilderExtensions
{
    public static void RemoveConsoleLogger(this ILoggingBuilder logging) =>
        LogProviderRemovalService.RemoveConsoleLogger(logging);

    public static void RemoveProvider(this ILoggingBuilder logging, Type providerType) =>
        LogProviderRemovalService.RemoveProvider(logging, providerType);
}