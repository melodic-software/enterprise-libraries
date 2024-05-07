using Microsoft.Extensions.Configuration;

namespace Enterprise.ModularMonoliths.Configuration;

public static class ConfigurationExtensions
{
    public static void AddModuleConfiguration(this IConfigurationBuilder configurationBuilder, string[] moduleNames, string[]? environmentNames = null)
    {
        environmentNames ??= ["Development", "Staging", "Production"];

        foreach (string moduleName in moduleNames)
        {
            configurationBuilder.AddJsonFile($"modules.{moduleName}.json", false, true);

            foreach (string environmentName in environmentNames)  
            {
                configurationBuilder.AddJsonFile($"modules.{moduleName}.{environmentName}.json", false, true);
            }
        }
    }
}
