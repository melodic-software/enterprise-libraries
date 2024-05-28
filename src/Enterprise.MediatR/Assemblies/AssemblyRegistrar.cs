using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Assemblies;

internal static class AssemblyRegistrar
{
    public static Assembly[] RegisterAssemblies()
    {
        // TODO: This fallback isn't ideal, as it could load a lot of assemblies we don't need.
        // We should try to find a more performant option to fall back to.
        PreStartupLogger.Instance.LogInformation("Explicit assemblies containing MediatR handlers have not been specified. Loading solution assemblies.");
        Assembly[] assemblies = AssemblyLoader.LoadSolutionAssemblies(AssemblyFilterPredicates.ThatAreNotMicrosoft);

        return assemblies;
    }

    public static Assembly[] RegisterExplicitAssemblies(Assembly[] assemblies)
    {
        PreStartupLogger.Instance.LogInformation("Registering MediatR handlers for the explicitly defined assemblies.");

        foreach (Assembly assembly in assemblies)
        {
            PreStartupLogger.Instance.LogInformation("{AssemblyName}", assembly.FullName);
        }

        return assemblies;
    }
}
