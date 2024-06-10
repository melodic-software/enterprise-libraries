using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using Enterprise.Options.Core.Services.Singleton;

namespace Enterprise.Logging.Providers;

internal static class ProviderConfigService
{
    internal static void ConfigureProviders(this IHostApplicationBuilder builder)
    {
        LoggingProviderOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<LoggingProviderOptions>(builder.Configuration, LoggingProviderOptions.ConfigSectionKey);

        builder.ConfigureProviders(options);
    }

    internal static void ConfigureProviders(this IHostApplicationBuilder builder, LoggingProviderOptions providerOptions)
    {
        builder.Logging.ClearProviders();

        if (providerOptions.EnableConsole)
        {
            builder.Logging.AddConsole();
        }

        if (providerOptions.EnableJsonConsole)
        {
            builder.Logging.AddJsonConsole();
        }

        if (providerOptions.EnableDebug)
        {
            builder.Logging.AddDebug();
        }

        if (providerOptions.EnableEventSource)
        {
            builder.Logging.AddEventSourceLogger();
        }

        // The event log is specific to Windows operating system.
        // TODO: We might want to add a filter here to ensure it only logs levels of "warning" or above.
        if (providerOptions.EnableEventLog && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.Logging.AddEventLog();
        }

        if (providerOptions.EnableApplicationInsights)
        {
            builder.Logging.AddApplicationInsights();
        }

        // This is application specific provider customization.
        providerOptions.ConfigureProviders(builder);
    }
}
