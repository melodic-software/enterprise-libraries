using System.Reflection;
using Enterprise.Reflection.Assemblies.Delegates;
using static Enterprise.Reflection.Assemblies.AssemblyLoadConstants;

namespace Enterprise.Reflection.Assemblies;

/// <summary>
/// Facilitates loading of assemblies based on specific criteria.
/// </summary>
public static class AssemblyLoader
{
    /// <summary>
    /// Loads all assemblies from the current application domain.
    /// This works at startup if you want to view ALL assemblies.
    /// Dynamically loaded assemblies are not returned.
    /// </summary>
    /// <returns></returns>
    public static List<Assembly> LoadAllAssemblies()
    {
        FilterAssemblyName filterPredicate = FilterAssemblyNameDefaults.NoFilter;
        List<Assembly> allAssemblies = LoadAllAssemblies(filterPredicate.Invoke);
        return allAssemblies;
    }

    /// <summary>
    /// Loads assemblies that match a given filter predicate.
    /// This works at startup if you want to view ALL assemblies.
    /// Dynamically loaded assemblies are not returned.
    /// The filter predicate is commonly used to apply a whitelist (ex: x => x.Name.StartsWith("MySolution")),
    /// or a blacklist (ex: x => !x.Name.StartsWith("System.").
    /// </summary>
    /// <param name="filterPredicate">A predicate used to determine which assemblies to load.</param>
    /// <returns>A list of loaded assemblies that match the filter predicate.</returns>
    public static List<Assembly> LoadAllAssemblies(Func<AssemblyName, bool> filterPredicate)
    {
        Queue<Assembly> assembliesToCheck = new();
        HashSet<string> loadedAssemblyNames = [];
        List<Assembly> assemblies = [];

        var entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly != null)
        {
            assembliesToCheck.Enqueue(entryAssembly);
        }

        while (assembliesToCheck.Any())
        {
            Assembly assemblyToCheck = assembliesToCheck.Dequeue();

            AssemblyName[] referencedAssemblyNames = assemblyToCheck.GetReferencedAssemblies();

            foreach (AssemblyName assemblyName in referencedAssemblyNames)
            {
                if (loadedAssemblyNames.Contains(assemblyName.FullName))
                {
                    continue;
                }

                bool doNotLoadAssembly = !filterPredicate?.Invoke(assemblyName) ?? false;

                if (doNotLoadAssembly)
                {
                    continue;
                }

                var assembly = Assembly.Load(assemblyName);
                assembliesToCheck.Enqueue(assembly);
                loadedAssemblyNames.Add(assemblyName.FullName);
                assemblies.Add(assembly);
            }
        }

        return assemblies;
    }

    /// <summary>
    /// Load all assemblies in the base directory of the current app domain.
    /// This will return dynamically loaded assemblies as long as they are under the aforementioned directory.
    /// </summary>
    /// <returns></returns>
    public static Assembly[] LoadSolutionAssemblies()
    {
        FilterAssemblyName filterPredicate = FilterAssemblyNameDefaults.NoFilter;
        Assembly[] assemblies = LoadSolutionAssemblies(filterPredicate);
        return assemblies;
    }

    /// <summary>
    /// Load all assemblies in the base directory of the current app domain.
    /// This will return dynamically loaded assemblies as long as they are under the aforementioned directory.
    /// The filter predicate is commonly used to apply a whitelist (ex: x => x.Name.StartsWith("MySolution"))
    /// or a blacklist (ex: x => !x.Name.StartsWith("System.").
    /// </summary>
    /// <param name="filterPredicate"></param>
    /// <returns></returns>
    public static Assembly[] LoadSolutionAssemblies(FilterAssemblyName filterPredicate)
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        string baseDirectory = currentDomain.BaseDirectory;
        string[] dllFiles = Directory.GetFiles(baseDirectory, DllSearchPattern);
        var assemblyNames = dllFiles.Select(AssemblyName.GetAssemblyName).ToList();
        var assemblyNamesToLoad = assemblyNames.Where(filterPredicate.Invoke).ToList();
        var assemblies = assemblyNamesToLoad.Select(Assembly.Load).ToList();
        return assemblies.ToArray();
    }
}
