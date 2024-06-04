using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Reflection.Assemblies.Delegates;
using Enterprise.Reflection.Attributes.Assemblies;
using Microsoft.Extensions.Logging;

namespace Enterprise.Applications.DI.Startup;

/// <summary>
/// Processes assembly paths and loads assemblies that meet specific criteria.
/// </summary>
internal static class AssemblyPathProcessor
{
    private const string AutoLoadAttributeName = nameof(AutoLoadAttribute);

    /// <summary>
    /// Processes the specified assembly paths and loads them using a filter predicate and a metadata load context.
    /// </summary>
    /// <param name="filterPredicate">A function to filter assembly names for loading.</param>
    /// <param name="assemblyPaths">The paths of assemblies to consider.</param>
    /// <param name="metadataLoadContext">The context used for loading assembly metadata without affecting the main application domain.</param>
    /// <returns>An array of successfully loaded assemblies.</returns>
    internal static Assembly[] ProcessAssemblyPaths(AssemblyNameFilter? filterPredicate, string[] assemblyPaths, MetadataLoadContext metadataLoadContext)
    {
        var loadedAssemblies = new ConcurrentBag<Assembly>();

        Parallel.ForEach(assemblyPaths, assemblyPath =>
        {
            try
            {
                if (!File.Exists(assemblyPath))
                {
                    throw new FileNotFoundException($"Assembly path \"{assemblyPath}\" does not exist.");
                }

                AssemblyName assemblyName = AssemblyLoadContext.GetAssemblyName(assemblyPath);

                if (SkipAssembly(filterPredicate, assemblyPath, metadataLoadContext, assemblyName))
                {
                    return;
                }

                var loadedAssembly = Assembly.Load(assemblyName);
                loadedAssemblies.Add(loadedAssembly);
                PreStartupLogger.Instance.LogDebug("Successfully loaded assembly: {AssemblyName}.", loadedAssembly.FullName);
            }
            catch (Exception ex)
            {
                PreStartupLogger.Instance.LogError(ex, "Failed to load assembly from path: {AssemblyPath}.", assemblyPath);
                throw;
            }
        });

        return loadedAssemblies.ToArray();
    }

    /// <summary>
    /// Determines whether an assembly should be skipped based on the filter predicate, its path, metadata context, and its name.
    /// </summary>
    /// <param name="filterPredicate">The filter predicate applied to assembly names.</param>
    /// <param name="assemblyPath">The file path of the assembly.</param>
    /// <param name="metadataLoadContext">The metadata load context for assembly inspection.</param>
    /// <param name="assemblyName">The name of the assembly being considered.</param>
    /// <returns>True if the assembly should be skipped; otherwise, false.</returns>
    private static bool SkipAssembly(AssemblyNameFilter? filterPredicate, string assemblyPath, MetadataLoadContext metadataLoadContext, AssemblyName assemblyName)
    {
        // Allow for early filtering on the assembly name.
        if (filterPredicate != null && !filterPredicate(assemblyName))
        {
            PreStartupLogger.Instance.LogDebug("Skipping assembly because it does not meet filter criteria: {AssemblyName}.", assemblyName);
            return true;
        }

        // This next line loads only the metadata of the assembly into the MetadataLoadContext.
        // It does not load the assembly into the application domain.
        // It allows inspection of types, attributes, and other metadata without the side effects that come with full assembly loading (such as executing static constructors).
        Assembly metadataAssembly = metadataLoadContext.LoadFromAssemblyPath(assemblyPath);

        IList<CustomAttributeData> customAttributes = metadataAssembly.GetCustomAttributesData();
        bool hasAutoLoadAttribute = customAttributes.Any(ad => ad.AttributeType.Name == AutoLoadAttributeName);

        if (hasAutoLoadAttribute)
        {
            return false;
        }

        PreStartupLogger.Instance.LogDebug("Skipping assembly without AutoLoadAttribute: {AssemblyName}.", assemblyName);
        
        return true;
    }
}
