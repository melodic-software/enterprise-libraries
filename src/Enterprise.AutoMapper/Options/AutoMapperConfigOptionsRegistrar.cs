using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.AutoMapper.Options;

public class AutoMapperConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<AutoMapperConfigOptions>(configuration, AutoMapperConfigOptions.ConfigSectionKey);
    }
}