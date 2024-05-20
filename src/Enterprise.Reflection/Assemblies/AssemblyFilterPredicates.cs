using System.Reflection;

namespace Enterprise.Reflection.Assemblies;

/// <summary>
/// Provides common predicate functions for filtering assemblies based on their names.
/// </summary>
public static class AssemblyFilterPredicates
{
    public static Func<AssemblyName, bool> NameStartsWithEnterprise => x =>
        !string.IsNullOrWhiteSpace(x.Name) &&
        x.Name.StartsWith("Enterprise", StringComparison.Ordinal);

    public static Func<AssemblyName, bool> NoFilter => _ => true;

    public static Func<AssemblyName, bool> ThatAreNotMicrosoft => x =>
        !string.IsNullOrWhiteSpace(x.Name) &&
        !x.Name.StartsWith("Microsoft", StringComparison.Ordinal) &&
        !x.Name.StartsWith("System", StringComparison.Ordinal);
}
