using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.InteropServices;
using static Enterprise.Reflection.Assemblies.AssemblyLoadConstants;

namespace Enterprise.Applications.DI.Startup;

/// <summary>
/// Provides a way to create and manage a metadata load context that facilitates assembly inspection without loading them into the application domain.
/// </summary>
internal static class MetadataLoadContextService
{
    /// <summary>
    /// Creates a metadata load context for the specified assembly paths.
    /// This allows inspection of assemblies without loading them into the main application domain.
    /// </summary>
    /// <param name="assemblyPaths">Paths to assemblies to be loaded for inspection.</param>
    /// <returns>A new metadata load context configured with specified assemblies.</returns>
    internal static MetadataLoadContext CreateMetadataLoadContext(string[] assemblyPaths)
    {
        // Get the path to the runtime assemblies (e.g., System.Private.CoreLib.dll, etc.)
        string runtimeAssemblyDirectory = Path.GetDirectoryName(typeof(object).Assembly.Location) ?? RuntimeEnvironment.GetRuntimeDirectory();
        string[] runtimeAssemblies = Directory.GetFiles(runtimeAssemblyDirectory, DllSearchPattern);

        // Depending on the application, some additional framework assemblies may need to be added here.
        string[] sharedFrameworkAssemblies = GetSharedFrameworkAssemblies();

        // Consolidate paths and ensure uniqueness.
        string[] allAssemblyPaths = assemblyPaths
            .Concat(runtimeAssemblies)
            .Concat(sharedFrameworkAssemblies)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        PathAssemblyResolver resolver = new PathAssemblyResolver(allAssemblyPaths);

        return new MetadataLoadContext(resolver);
    }

    private static string[] GetSharedFrameworkAssemblies()
    {
        IReadOnlyCollection<string> sharedFrameworkDirectories = SharedFrameworkAssemblyService.Instance.SharedFrameworkDirectories;

        // Ensure we do not process the same directory twice.
        sharedFrameworkDirectories = sharedFrameworkDirectories.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

        List<string> sharedFrameworkAssemblies = [];

        if (!sharedFrameworkDirectories.Any())
            return sharedFrameworkAssemblies.ToArray();

        string metadataLoadContextName = nameof(MetadataLoadContext);

        PreStartupLogger.Instance.LogDebug($"Adding shared framework directories for {metadataLoadContextName}.");

        foreach (string? sharedFrameworkDirectory in sharedFrameworkDirectories)
        {
            if (string.IsNullOrWhiteSpace(sharedFrameworkDirectory))
            {
                PreStartupLogger.Instance.LogWarning($"Shared framework directory is invalid for {metadataLoadContextName}: {sharedFrameworkDirectory}");
                continue;
            }

            PreStartupLogger.Instance.LogDebug($"Adding shared framework assembly directory for {metadataLoadContextName}: {sharedFrameworkDirectory}");
            string[] sharedFrameworkDirectoryAssemblies = Directory.GetFiles(sharedFrameworkDirectory, DllSearchPattern);
            sharedFrameworkAssemblies.AddRange(sharedFrameworkDirectoryAssemblies);
        }

        string[] result = sharedFrameworkAssemblies.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

        return result;
    }
}