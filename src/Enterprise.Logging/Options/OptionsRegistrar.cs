using Enterprise.Logging.Providers;
using Enterprise.Logging.TraceListeners;
using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.Options;

internal sealed class OptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<LoggingOptions>(configuration, LoggingOptions.ConfigSectionKey);
        services.RegisterOptions<LoggingProviderOptions>(configuration, LoggingProviderOptions.ConfigSectionKey);
        services.RegisterOptions<TraceListenerOptions>(configuration, TraceListenerOptions.ConfigSectionKey);
    }
}
