using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Quartz.Options;

public class QuartzConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<QuartzConfigOptions>(configuration, QuartzConfigOptions.ConfigSectionKey);
    }
}