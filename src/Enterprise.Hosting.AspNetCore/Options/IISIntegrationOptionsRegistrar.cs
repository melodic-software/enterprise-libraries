using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Hosting.AspNetCore.Options;

public class IISIntegrationOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<IISIntegrationOptions>(configuration, IISIntegrationOptions.ConfigSectionKey);
    }
}