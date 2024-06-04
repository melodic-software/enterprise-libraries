namespace Enterprise.Reflection.Assemblies.Delegates;

/// <summary>
/// Provides common predicate functions for filtering assemblies based on their names.
/// </summary>
public static class AssemblyNameFilters
{
    public static AssemblyNameFilter NameStartsWithEnterprise => x =>
        !string.IsNullOrWhiteSpace(x.Name) &&
        x.Name.StartsWith("Enterprise", StringComparison.Ordinal);

    public static AssemblyNameFilter NoFilter => _ => true;

    public static AssemblyNameFilter ThatAreNotMicrosoft => x =>
        !string.IsNullOrWhiteSpace(x.Name) &&
        !x.Name.StartsWith("Microsoft", StringComparison.Ordinal) &&
        !x.Name.StartsWith("System", StringComparison.Ordinal);
}
