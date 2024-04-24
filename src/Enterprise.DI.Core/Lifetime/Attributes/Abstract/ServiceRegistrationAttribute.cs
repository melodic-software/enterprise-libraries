namespace Enterprise.DI.Core.Lifetime.Attributes.Abstract;

/// <summary>
/// Abstract attribute to define common properties for service lifetime registration.
/// </summary>
public abstract class ServiceRegistrationAttribute : Attribute
{
    /// <summary>
    /// Gets a value indicating whether the class should be registered as a service for its matching interface only.
    /// When set to true, the class is registered as a service for the interface that matches its name, 
    /// ignoring other implemented interfaces.
    /// </summary>
    public bool AsMatchingInterface { get; } = false;

    /// <summary>
    /// Gets a value indicating whether the class should be registered as a service for all interfaces it implements.
    /// This is the default behavior. When set to true, the class is registered as a service for each interface it implements.
    /// </summary>
    public bool AsImplementedInterfaces { get; } = true;

    /// <summary>
    /// Gets a value indicating whether the class should be registered as a service under its own type.
    /// When set to true, the class is registered as a service under its own type.
    /// </summary>
    public bool AsSelf { get; } = true;
}