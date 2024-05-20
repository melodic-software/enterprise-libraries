using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Logging.Options;

namespace Enterprise.Api.Logging;

internal sealed class DefaultLoggingConfigOptions : LoggingConfigOptions
{
    internal DefaultLoggingConfigOptions()
    {
        ConfigureExtendedServices = LoggingMiddlewareService.RegisterLoggingMiddlewareServices;
    }
}