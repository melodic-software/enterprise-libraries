using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Controllers.Options;

internal sealed class ControllerOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<ControllerOptions>(configuration, ControllerOptions.ConfigSectionKey);
    }
}
