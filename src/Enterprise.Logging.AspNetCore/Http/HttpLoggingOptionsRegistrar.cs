using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;

namespace Enterprise.Logging.AspNetCore.Http;

internal sealed class HttpLoggingOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<HttpLoggingOptions>(configuration, HttpLoggingOptions.ConfigSectionKey);
    }
}
