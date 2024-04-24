using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.AspNetCore.W3C;

public class W3CLoggingConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<W3CLoggingConfigOptions>(configuration, W3CLoggingConfigOptions.ConfigSectionKey);
    }
}