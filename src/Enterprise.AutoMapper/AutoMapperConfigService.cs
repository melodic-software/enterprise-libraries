using System.Reflection;
using Enterprise.AutoMapper.Assemblies;
using Enterprise.AutoMapper.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.AutoMapper.Assemblies.AssemblyFallbackService;
using static Enterprise.AutoMapper.Assemblies.ExplicitAssemblyLogger;

namespace Enterprise.AutoMapper;

public static class AutoMapperConfigService
{
    public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        AutoMapperOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<AutoMapperOptions>(configuration, AutoMapperOptions.ConfigSectionKey);

        if (!options.EnableAutoMapper)
        {
            return;
        }

        List<Assembly> assemblies = AutoMapperAssemblyService.Instance.AssembliesToRegister;
        bool explicitAssembliesSpecified = assemblies.Any();

        if (explicitAssembliesSpecified)
        {
            LogExplicitAssemblies(assemblies);
        }
        else
        {
            assemblies = GetAssemblies();
        }

        AddEnterpriseAssembly(assemblies);

        services.AddAutoMapper(options.Configure, [..assemblies]);
    }

    private static void AddEnterpriseAssembly(List<Assembly> allAssemblies)
    {
        // We have profile mappings in this assembly that are not application specific, and should always be included.
        Assembly enterpriseAssembly = typeof(AutoMapperConfigService).Assembly;

        if (allAssemblies.TrueForAll(a => a.GetName().FullName != enterpriseAssembly.FullName))
        {
            allAssemblies.Add(enterpriseAssembly);
        }
    }
}
