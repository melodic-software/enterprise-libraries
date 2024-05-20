using Enterprise.Options.Core.Singleton;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Enterprise.Logging.Providers;

internal static class ProviderConfigService
{
    internal static void ConfigureProviders(this IHostApplicationBuilder builder)
    {
        LoggingProviderConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<LoggingProviderConfigOptions>(builder.Configuration, LoggingProviderConfigOptions.ConfigSectionKey);

        builder.ConfigureProviders(configOptions);
    }

    internal static void ConfigureProviders(this IHostApplicationBuilder builder, LoggingProviderConfigOptions configOptions)
    {
        builder.Logging.ClearProviders();

        if (configOptions.EnableConsole)
        {
            builder.Logging.AddConsole();
        }

        if (configOptions.EnableJsonConsole)
        {
            builder.Logging.AddJsonConsole();
        }

        if (configOptions.EnableDebug)
        {
            builder.Logging.AddDebug();
        }

        if (configOptions.EnableEventSource)
        {
            builder.Logging.AddEventSourceLogger();
        }

        // The event log is specific to Windows operating system.
        // TODO: We might want to add a filter here to ensure it only logs levels of "warning" or above.
        if (configOptions.EnableEventLog && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.Logging.AddEventLog();
        }

        if (configOptions.EnableApplicationInsights)
        {
            builder.Logging.AddApplicationInsights();
        }

        // This is application specific provider customization.
        configOptions.ConfigureProviders.Invoke(builder);
    }
}
