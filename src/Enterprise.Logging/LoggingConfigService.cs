using Enterprise.Logging.Filters;
using Enterprise.Logging.Options;
using Enterprise.Logging.Providers;
using Enterprise.Logging.TraceListeners;
using Enterprise.TextEncoding.ConsoleEncoding;
using Microsoft.Extensions.Hosting;
using System.Text;
using Enterprise.Options.Core.Services;

namespace Enterprise.Logging;

public static class LoggingConfigService
{
    public static void ConfigureLogging(this IHostApplicationBuilder builder)
    {
        LoggingConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<LoggingConfigOptions>(builder.Configuration, LoggingConfigOptions.ConfigSectionKey);

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging

        // Completely customize the logging config.
        if (configOptions.CustomConfigureLogging != null)
        {
            configOptions.CustomConfigureLogging(builder.Logging);
            return;
        }

        // Allow for unicode characters to be printed to the console.
        // Without this they would be shown as the unicode "code" ex: \u0022a
        Console.OutputEncoding = Encoding.UTF8;
        ConsoleEncodingService.AllowSpecialCharacters();

        // This configures the standard .NET logging providers.
        builder.ConfigureProviders();

        // These are additional logging outputs.
        builder.ConfigureTraceListeners();

        // Allow for consuming applications to register extended services.
        // One example would be in an ASP.NET Core application (middleware service registrations).
        configOptions.ConfigureExtendedServices?.Invoke(builder);

        // These allow for configuration of log filters for specific providers, categories, levels, etc.
        // For the most part, these should be done through application settings (JSON files).
        builder.ConfigureFilters(configOptions);
    }
}
