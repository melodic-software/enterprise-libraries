using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Controllers.Options;

public class ControllerConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<ControllerConfigOptions>(configuration, ControllerConfigOptions.ConfigSectionKey);
    }
}