using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Logging.Options;

namespace Enterprise.Api.Logging;

internal sealed class DefaultLoggingOptions : LoggingOptions
{
    internal DefaultLoggingOptions()
    {
        ConfigureExtendedServices = LoggingMiddlewareService.RegisterLoggingMiddlewareServices;
    }
}
