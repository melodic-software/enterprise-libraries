using System.Reflection;

namespace Enterprise.Reflection.Assemblies;

/// <summary>
/// Provides functionality to retrieve already loaded assemblies from the current application domain.
/// </summary>
public static class LoadedAssemblyService
{
    /// <summary>
    /// Retrieves all assemblies that are currently loaded in the application domain.
    /// Assemblies are lazily loaded into the app domain.
    /// This means it is possible that not all assemblies will be returned depending on when this method is called.
    /// Assemblies will not be returned unless a call has been made to a method or class in the assembly.
    /// </summary>
    /// <returns>An array of assemblies that are currently loaded.</returns>
    public static Assembly[] GetFromCurrentAppDomain()
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        Assembly[] assemblies = currentDomain.GetAssemblies();
        return assemblies;
    }

    public static string[] GetNamesFromCurrentAppDomain()
    {
        Assembly[] loadedAssemblies = GetFromCurrentAppDomain();

        string[] assemblyNames = loadedAssemblies
            .Select(asm => asm.GetName().Name ?? string.Empty)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return assemblyNames;
    }
}
