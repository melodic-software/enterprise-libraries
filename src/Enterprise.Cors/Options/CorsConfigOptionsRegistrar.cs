using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Cors.Options;

public class CorsConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<CorsConfigOptions>(configuration, CorsConfigOptions.ConfigSectionKey);
    }
}