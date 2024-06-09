using System.Text;
using Enterprise.Logging.Filters;
using Enterprise.Logging.Options;
using Enterprise.Logging.Providers;
using Enterprise.Logging.TraceListeners;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.TextEncoding.ConsoleEncoding;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Logging.Config;

public static class LoggingConfigService
{
    public static void ConfigureLogging(this IHostApplicationBuilder builder)
    {
        LoggingOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<LoggingOptions>(builder.Configuration, LoggingOptions.ConfigSectionKey);

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging

        // Completely customize the logging config.
        if (options.CustomConfigureLogging != null)
        {
            options.CustomConfigureLogging(builder.Logging);
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
        options.ConfigureExtendedServices?.Invoke(builder);

        // These allow for configuration of log filters for specific providers, categories, levels, etc.
        // For the most part, these should be done through application settings (JSON files).
        builder.ConfigureFilters(options);
    }
}
