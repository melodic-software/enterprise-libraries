using Enterprise.Options.Core.Services.Singleton;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Logging.AspNetCore.Telemetry;

internal static class TelemetryConfigService
{
    internal static void ConfigureTelemetry(this IHostApplicationBuilder builder)
    {
        TelemetryOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<TelemetryOptions>(builder.Configuration, TelemetryOptions.ConfigSectionKey);

        builder.ConfigureTelemetry(options);
    }

    internal static void ConfigureTelemetry(this IHostApplicationBuilder builder, TelemetryOptions options)
    {
        if (!options.EnableApplicationInsightsTelemetry)
        {
            return;
        }

        builder.Services.AddApplicationInsightsTelemetry();
    }
}
