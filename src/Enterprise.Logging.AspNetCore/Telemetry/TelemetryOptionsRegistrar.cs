using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.AspNetCore.Telemetry;

internal sealed class TelemetryOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<TelemetryOptions>(configuration, TelemetryOptions.ConfigSectionKey);
    }
}
