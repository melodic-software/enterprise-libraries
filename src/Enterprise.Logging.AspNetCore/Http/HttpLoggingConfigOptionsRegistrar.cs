using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;

namespace Enterprise.Logging.AspNetCore.Http;

public class HttpLoggingConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<HttpLoggingConfigOptions>(configuration, HttpLoggingConfigOptions.ConfigSectionKey);
    }
}