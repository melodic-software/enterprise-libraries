namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql.PropertyConfig;

/// <summary>
/// Base configuration for audit date properties in an entity.
/// </summary>
public abstract record AuditDatePropertyConfig
{
    /// <summary>
    /// This is a SQL expression to resolve a default value.
    /// </summary>
    public string? DefaultSql { get; init; }

    /// <summary>
    /// Default values are not supplied to nullable properties unless explicitly specified.
    /// </summary>
    public bool ApplyDefaultToNullable { get; init; }

    /// <summary>
    /// Determines if this is a shadow property.
    /// This will need to be set if the model does not have a property member matching the name.
    /// </summary>
    public bool IsShadowProperty { get; init; }

    /// <summary>
    /// This is the name of the entity property.
    /// </summary>
    public abstract string PropertyName { get; }

    /// <summary>
    /// Defines the nullability.
    /// This is mostly geared towards shadow properties.
    /// For regular properties, reflection is used to determine property type nullability.
    /// If the property type nullability can be determined, this value is ignored.
    /// </summary>
    public bool IsNullable { get; init; }
}