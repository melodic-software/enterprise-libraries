using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.ModularMonoliths.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.ModularMonoliths.ModuleNaming;

internal sealed class ModuleStateServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IModuleNameService>(provider =>
        {
            IOptions<ModuleNamingOptions> moduleNamingOptions = provider.GetRequiredService<IOptions<ModuleNamingOptions>>();
            return new ModuleNameService(moduleNamingOptions);
        });
    }
}
