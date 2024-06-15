using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;

namespace Enterprise.AutoMapper.Assemblies;

internal static class ExplicitAssemblyLogger
{
    public static List<Assembly> LogExplicitAssemblies(List<Assembly> assemblies)
    {
        PreStartupLogger.Instance.LogInformation("Registering mapping profiles for the explicitly defined assemblies.");

        foreach (Assembly assembly in assemblies)
        {
            PreStartupLogger.Instance.LogInformation("{AssemblyName}", assembly.FullName);
        }

        return assemblies;
    }
}
