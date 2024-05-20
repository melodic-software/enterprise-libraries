using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Providers;

public static class LogProviderRemovalService
{
    public static void RemoveConsoleLogger(ILoggingBuilder logging)
    {
        Type providerType = typeof(ConsoleLoggerProvider);
        RemoveProvider(logging, providerType);
    }

    public static void RemoveProvider(ILoggingBuilder logging, Type providerType)
    {
        foreach (ServiceDescriptor serviceDescriptor in logging.Services)
        {
            if (serviceDescriptor.ImplementationType != providerType)
            {
                continue;
            }

            logging.Services.Remove(serviceDescriptor);

            break;
        }
    }
}
