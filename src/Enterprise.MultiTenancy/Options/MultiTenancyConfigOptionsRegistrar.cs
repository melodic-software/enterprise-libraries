using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Multitenancy.Options;

public class MultiTenancyConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<MultiTenancyConfigOptions>(configuration, MultiTenancyConfigOptions.ConfigSectionKey);
    }
}
