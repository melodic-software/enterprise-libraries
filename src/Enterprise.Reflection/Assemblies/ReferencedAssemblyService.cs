using System.Reflection;

namespace Enterprise.Reflection.Assemblies;

/// <summary>
/// Provides methods to access assemblies referenced by the entry assembly.
/// </summary>
public static class ReferencedAssemblyService
{
    /// <summary>
    /// Retrieves the assemblies directly referenced by the entry assembly of the current application.
    /// Typically this is the main application (web project, API, windows service, etc.).
    /// This only works with direct references. It doesn't pull in chained references.
    /// </summary>
    /// <returns></returns>
    public static AssemblyName[]? GetEntryAssemblyReferences()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        AssemblyName[]? referencedAssemblies = entryAssembly?.GetReferencedAssemblies();
        return referencedAssemblies;
    }
}
