using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Logging;
using static Enterprise.Reflection.Assemblies.Delegates.AssemblyNameFilters;

namespace Enterprise.MediatR.Assemblies;

internal static class AssemblyFallbackService
{
    public static List<Assembly> GetAssemblies()
    {
        // TODO: This fallback isn't ideal, as it could load a lot of assemblies we don't need.
        // We should try to find a more performant option to fall back to.
        PreStartupLogger.Instance.LogInformation("Explicit assemblies containing MediatR handlers have not been specified. Loading solution assemblies.");
        Assembly[] assemblies = AssemblyLoader.LoadSolutionAssemblies(ThatAreNotMicrosoft);
        return assemblies.ToList();
    }
}
