using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.ModularMonolith;

internal sealed class ModularMonolithOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<ModularMonolithOptions>(configuration, ModularMonolithOptions.ConfigSectionKey);
    }
}
