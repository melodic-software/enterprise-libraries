using Enterprise.DI.Core.Lifetime.Attributes.Abstract;

namespace Enterprise.DI.Core.Lifetime.Attributes;

/// <summary>
/// Attribute to indicate that a class should be registered with a singleton lifetime in the DI container.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class SingletonServiceAttribute : ServiceRegistrationAttribute;