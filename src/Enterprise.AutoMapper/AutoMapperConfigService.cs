using Enterprise.AutoMapper.Options;
using Enterprise.Options.Core.Singleton;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using static Enterprise.Reflection.Assemblies.AssemblyFilterPredicates;

namespace Enterprise.AutoMapper;

public static class AutoMapperConfigService
{
    public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        AutoMapperConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<AutoMapperConfigOptions>(configuration, AutoMapperConfigOptions.ConfigSectionKey);

        if (!options.EnableAutoMapper)
            return;

        // TODO: This isn't a great fallback, as it could load a ton of assemblies.
        // We should try and provide a more performant fallback.
        options.GetMappingProfileAssemblies ??= GetDefaultAssemblies;

        Assembly[] allAssemblies = options.GetMappingProfileAssemblies.Invoke();

        allAssemblies = AddEnterpriseAssembly(allAssemblies);

        services.AddAutoMapper(allAssemblies);
    }

    private static Assembly[] GetDefaultAssemblies()
    {
        return AssemblyLoader.LoadSolutionAssemblies(ThatAreNotMicrosoft).ToArray();
    }

    private static Assembly[] AddEnterpriseAssembly(Assembly[] allAssemblies)
    {
        // We have profile mappings in this assembly that are not application specific.
        Assembly enterpriseMappingAssembly = typeof(AutoMapperConfigService).Assembly;

        if (allAssemblies.All(a => a.FullName != enterpriseMappingAssembly.FullName))
            allAssemblies = [..allAssemblies, enterpriseMappingAssembly];

        return allAssemblies;
    }
}