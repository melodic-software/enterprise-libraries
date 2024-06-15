namespace Enterprise.Reflection.Attributes.Assemblies;

/// <summary>
/// Indicates that an assembly should be automatically loaded.
/// This is primarily used to help target specific assemblies when executing dynamic behavior.
/// Some examples of this are dynamic options, service, and middleware registrations.
/// Using the attribute provides an assembly metadata target for scanning and inspection,
/// which is less expensive than loading all assemblies.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class AutoLoadAttribute : Attribute;
