using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.AspNetCore.W3C;

internal sealed class OptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<W3CLoggingOptions>(configuration, W3CLoggingOptions.ConfigSectionKey);
    }
}
