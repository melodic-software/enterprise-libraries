using Enterprise.Api.Logging;
using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Logging.Providers;
using Enterprise.Logging.Options;
using Enterprise.Options.Core.Singleton;
using Enterprise.Serilog.Options;

namespace Enterprise.Api.Options;

internal static class DefaultOptionsService
{
    /// <summary>
    /// Specify default object instances.
    /// </summary>
    internal static void RegisterDefaults()
    {
        OptionsInstanceService.Instance.ConfigureDefaultInstance<LoggingConfigOptions>(new DefaultLoggingConfigOptions());
        OptionsInstanceService.Instance.ConfigureDefaultInstance<LoggingProviderConfigOptions>(new DefaultLoggingProviderConfigOptions());
        OptionsInstanceService.Instance.ConfigureDefaultInstance<LoggingMiddlewareConfigOptions>(new DefaultLoggingMiddlewareConfigOptions());
        OptionsInstanceService.Instance.ConfigureDefaultInstance<SerilogConfigOptions>(new DefaultSerilogConfigOptions());
        // Register any other defaults here.
    }
}
