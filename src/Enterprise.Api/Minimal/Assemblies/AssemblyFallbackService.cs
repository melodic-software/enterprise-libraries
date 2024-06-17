using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Logging;
using static Enterprise.Reflection.Assemblies.Delegates.AssemblyNameFilters;

namespace Enterprise.Api.Minimal.Assemblies;

internal static class AssemblyFallbackService
{
    public static List<Assembly> GetAssemblies()
    {
        PreStartupLogger.Instance.LogInformation(
            "Explicit assemblies containing minimal API endpoints have not been specified. " +
            "Loading solution assemblies."
        );

        var endpointAssemblies = AssemblyLoader
            .LoadSolutionAssemblies(ThatAreNotMicrosoft)
            .ToList();

        return endpointAssemblies;
    }
}
