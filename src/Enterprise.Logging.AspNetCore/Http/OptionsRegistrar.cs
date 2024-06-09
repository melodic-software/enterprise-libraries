using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;

namespace Enterprise.Logging.AspNetCore.Http;

internal sealed class OptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<HttpLoggingOptions>(configuration, HttpLoggingOptions.ConfigSectionKey);
    }
}
