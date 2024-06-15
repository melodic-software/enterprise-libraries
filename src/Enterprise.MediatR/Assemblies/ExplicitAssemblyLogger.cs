using System.Collections.Generic;
using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Assemblies;

internal static class ExplicitAssemblyLogger
{
    public static List<Assembly> LogExplicitAssemblies(List<Assembly> assemblies)
    {
        PreStartupLogger.Instance.LogInformation("Registering MediatR handlers for the explicitly defined assemblies.");

        foreach (Assembly assembly in assemblies)
        {
            PreStartupLogger.Instance.LogInformation("{AssemblyName}", assembly.FullName);
        }

        return assemblies;
    }
}
