using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.AspNetCore.Telemetry;

public class TelemetryConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<TelemetryConfigOptions>(configuration, TelemetryConfigOptions.ConfigSectionKey);
    }
}