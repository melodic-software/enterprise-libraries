using Enterprise.Logging.AspNetCore.Options;
using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.AspNetCore.Middleware;

public class LoggingMiddlewareConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<LoggingMiddlewareConfigOptions>(configuration, LoggingMiddlewareConfigOptions.ConfigSectionKey);
    }
}