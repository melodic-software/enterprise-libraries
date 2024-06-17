using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Minimal.Assemblies;

internal static class ExplicitAssemblyLogger
{
    public static void LogExplicitAssemblies(List<Assembly> endpointAssemblies)
    {
        PreStartupLogger.Instance.LogInformation("Registering minimal API endpoints for the explicitly defined assemblies.");

        foreach (Assembly assembly in endpointAssemblies)
        {
            PreStartupLogger.Instance.LogInformation("{AssemblyName}", assembly.FullName);
        }
    }
}
