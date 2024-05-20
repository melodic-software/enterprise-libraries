namespace Enterprise.Reflection.Attributes.Assemblies;

/// <summary>
/// Indicates that an assembly should be automatically loaded.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class AutoLoadAttribute : Attribute;