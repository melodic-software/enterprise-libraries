using System.Reflection;
using Enterprise.AutoMapper.Options;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Enterprise.Reflection.Assemblies.AssemblyFilterPredicates;

namespace Enterprise.AutoMapper;

public static class AutoMapperConfigService
{
    public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        AutoMapperConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<AutoMapperConfigOptions>(configuration, AutoMapperConfigOptions.ConfigSectionKey);

        if (!options.EnableAutoMapper)
        {
            return;
        }

        Assembly[] assemblies = options.Assemblies.ToArray();
        bool explicitAssembliesSpecified = assemblies.Any();

        if (!explicitAssembliesSpecified)
        {
            PreStartupLogger.Instance.LogInformation("Explicit assemblies containing automapper profiles have not been specified. Loading solution assemblies.");
            assemblies = AssemblyLoader.LoadSolutionAssemblies(ThatAreNotMicrosoft).ToArray();
        }

        assemblies = AddEnterpriseAssembly(assemblies);

        if (explicitAssembliesSpecified)
        {
            PreStartupLogger.Instance.LogInformation("Registering mapping profiles for the explicitly defined assemblies.");

            foreach (Assembly assembly in assemblies)
            {
                PreStartupLogger.Instance.LogInformation("{AssemblyName}", assembly.FullName);
            }
        }

        services.AddAutoMapper(assemblies);
    }

    private static Assembly[] AddEnterpriseAssembly(Assembly[] allAssemblies)
    {
        // We have profile mappings in this assembly that are not application specific.
        Assembly enterpriseMappingAssembly = typeof(AutoMapperConfigService).Assembly;

        if (Array.TrueForAll(allAssemblies, a => a.FullName != enterpriseMappingAssembly.FullName))
        {
            allAssemblies = [..allAssemblies, enterpriseMappingAssembly];
        }

        return allAssemblies;
    }
}
