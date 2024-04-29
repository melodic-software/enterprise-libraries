using Enterprise.Logging.Providers;
using Enterprise.Logging.TraceListeners;
using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.Options;

public class LoggingConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<LoggingConfigOptions>(configuration, LoggingConfigOptions.ConfigSectionKey);
        services.RegisterOptions<LoggingProviderConfigOptions>(configuration, LoggingProviderConfigOptions.ConfigSectionKey);
        services.RegisterOptions<TraceListenerConfigOptions>(configuration, TraceListenerConfigOptions.ConfigSectionKey);
    }
}