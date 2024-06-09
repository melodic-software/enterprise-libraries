using Enterprise.Api.Logging;
using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Logging.Providers;
using Enterprise.Logging.Options;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Serilog.Options;

namespace Enterprise.Api.Options;

internal static class DefaultOptionsService
{
    /// <summary>
    /// Specify default object instances.
    /// </summary>
    internal static void RegisterDefaults()
    {
        OptionsInstanceService.Instance.ConfigureDefaultInstance<LoggingOptions>(new DefaultLoggingOptions());
        OptionsInstanceService.Instance.ConfigureDefaultInstance<LoggingProviderOptions>(new DefaultLoggingProviderOptions());
        OptionsInstanceService.Instance.ConfigureDefaultInstance<LoggingMiddlewareOptions>(new DefaultLoggingMiddlewareOptions());
        OptionsInstanceService.Instance.ConfigureDefaultInstance<SerilogOptions>(new DefaultSerilogOptions());
        // Register any other defaults here.
    }
}
