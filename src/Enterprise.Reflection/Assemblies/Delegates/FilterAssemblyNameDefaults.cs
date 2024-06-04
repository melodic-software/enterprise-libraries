namespace Enterprise.Reflection.Assemblies.Delegates;

/// <summary>
/// Provides common predicate functions for filtering assemblies based on their names.
/// </summary>
public static class FilterAssemblyNameDefaults
{
    public static FilterAssemblyName NameStartsWithEnterprise => x =>
        !string.IsNullOrWhiteSpace(x.Name) &&
        x.Name.StartsWith("Enterprise", StringComparison.Ordinal);

    public static FilterAssemblyName NoFilter => _ => true;

    public static FilterAssemblyName ThatAreNotMicrosoft => x =>
        !string.IsNullOrWhiteSpace(x.Name) &&
        !x.Name.StartsWith("Microsoft", StringComparison.Ordinal) &&
        !x.Name.StartsWith("System", StringComparison.Ordinal);
}
