using Enterprise.Options.Core.Singleton;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Logging.AspNetCore.Telemetry;

internal static class TelemetryConfigService
{
    internal static void ConfigureTelemetry(this IHostApplicationBuilder builder)
    {
        TelemetryConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<TelemetryConfigOptions>(builder.Configuration, TelemetryConfigOptions.ConfigSectionKey);

        builder.ConfigureTelemetry(configOptions);
    }

    internal static void ConfigureTelemetry(this IHostApplicationBuilder builder, TelemetryConfigOptions configOptions)
    {
        if (!configOptions.EnableApplicationInsightsTelemetry)
        {
            return;
        }

        builder.Services.AddApplicationInsightsTelemetry();
    }
}
