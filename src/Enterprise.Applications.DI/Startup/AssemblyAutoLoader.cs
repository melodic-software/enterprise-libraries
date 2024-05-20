using Enterprise.Logging.Core.Loggers;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using static Enterprise.Reflection.Assemblies.AssemblyLoadConstants;

namespace Enterprise.Applications.DI.Startup;

/// <summary>
/// Provides functionality to automatically load assemblies based on specific conditions.
/// </summary>
public static class AssemblyAutoLoader
{
    private static readonly ConcurrentDictionary<string, Assembly[]> AssemblyCache = new();

    /// <summary>
    /// Loads assemblies based on a specified filter predicate. If an exception occurs during loading,
    /// the method will attempt to load all solution assemblies as a fallback.
    /// </summary>
    /// <param name="filterPredicate">A function to determine which assemblies should be loaded. If null, all assemblies are considered.</param>
    /// <returns>An array of loaded assemblies.</returns>
    public static Assembly[] LoadAssemblies(Func<AssemblyName, bool>? filterPredicate = null)
    {
        PreStartupLogger.Instance.LogInformation("Auto loading assemblies.");

        // Generate a key based on the filter predicate to use for caching.
        string cacheKey = filterPredicate?.Method.ToString() ?? "All";

        // Check if the assemblies are already loaded and cached.
        if (AssemblyCache.TryGetValue(cacheKey, out Assembly[]? cachedAssemblies))
        {
            PreStartupLogger.Instance.LogInformation("Returning cached assemblies.");
            return cachedAssemblies;
        }

        var stopwatch = Stopwatch.StartNew();

        Assembly[]? loadedAssemblies;

        try
        {
            // Get assemblies in the base directory.
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] assemblyPaths = Directory.GetFiles(baseDirectory, DllSearchPattern);

            using MetadataLoadContext metadataContext = MetadataLoadContextService.CreateMetadataLoadContext(assemblyPaths);

            loadedAssemblies = AssemblyPathProcessor.ProcessAssemblyPaths(filterPredicate, assemblyPaths, metadataContext);
        }
        catch (Exception ex)
        {
            // Attempt to load all solution assemblies as a fallback to ensure system stability when specific assemblies fail to load.
            PreStartupLogger.Instance.LogError(ex, "An exception occurred while auto loading assemblies. Falling back to loading all solution assemblies.");

            // This is less performant, but we can safely fall back to this if needed.
            filterPredicate ??= name => true;
            //filterPredicate ??= AssemblyFilterPredicates.NoFilter;

            loadedAssemblies = AssemblyLoader.LoadSolutionAssemblies(filterPredicate);
        }

        // Cache the loaded assemblies.
        AssemblyCache.TryAdd(cacheKey, loadedAssemblies);

        stopwatch.Stop();

        PreStartupLogger.Instance.LogInformation("Assembly loading completed in {Milliseconds} ms.", stopwatch.ElapsedMilliseconds);

        loadedAssemblies = [.. loadedAssemblies.OrderBy(x => x.FullName)];

        return loadedAssemblies;
    }
}