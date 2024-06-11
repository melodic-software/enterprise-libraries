using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Serilog.Options;

internal sealed class SerilogOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<SerilogOptions>(configuration, SerilogOptions.ConfigSectionKey);
    }
}
