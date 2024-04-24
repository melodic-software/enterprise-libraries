using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql;

public static class StandardKeyPropertyExtensions
{
    /// <summary>
    /// Configures standard key properties for an entity, including a shadow primary key and an alternate key.
    /// The shadow primary key is the typical auto-incrementing integer PK used in relational databases.
    /// We typically use a GUID property on the model as an alternate key in the database, but others can be provided.
    /// This is done to prevent exposure of internal database implementation details and to optimize SQL performance.
    /// External systems would also reference this GUID and not the database integer PK.
    /// </summary>
    /// <typeparam name="T">The entity type being configured.</typeparam>
    /// <param name="builder">The builder providing a fluent API for configuring the entity.</param>
    /// <param name="primaryKeyPropertyName">The name of the shadow property to be used as the primary key.</param>
    /// <param name="alternateKeyPropertySelector">An expression to select property on the entity that will be used as the alternate key.</param>
    /// <param name="alternateKeyColumnName">The name for the column when creating the alternate key, if different from the default.</param>
    /// <param name="configureAlternateKeyProperty">Allows for further configuration of the alternate key.</param>
    public static EntityTypeBuilder<T> HasStandardKeys<T>(
        this EntityTypeBuilder<T> builder,
        string primaryKeyPropertyName,
        Expression<Func<T, object?>> alternateKeyPropertySelector,
        string? alternateKeyColumnName = null,
        Action<PropertyBuilder<object?>>? configureAlternateKeyProperty = null)
        where T : class
    {
        builder.HasPrimaryKey(null, primaryKeyPropertyName);
        builder.HasAlternateKey(alternateKeyPropertySelector, alternateKeyColumnName, configureAlternateKeyProperty);
        return builder;
    }

    /// <summary>
    /// Configures the standard keys for an entity, defining both the primary and alternate keys using provided selectors.
    /// </summary>
    /// <typeparam name="T">The type of the entity being configured.</typeparam>
    /// <param name="builder">The builder used to configure the entity.</param>
    /// <param name="primaryKeyPropertySelector">An expression that identifies the property to be used as the primary key.</param>
    /// <param name="alternateKeyPropertySelector">An expression that identifies the property to be used as the alternate key.</param>
    /// <param name="alternateKeyColumnName">Optional. Specifies the column name for the alternate key in the database. If not provided, a default name is used.</param>
    /// <param name="configureAlternateKeyProperty">Optional. A callback to further configure the alternate key property, such as setting column type or constraints.</param>
    /// <remarks>
    /// This method simplifies the key configuration process by allowing the primary and alternate keys to be set in a single call.
    /// The primary key is typically an auto-incrementing integer, while the alternate key is often a GUID that can be exposed to external systems.
    /// This method ensures that the entity's keys are configured according to standard practices, with the alternate key often serving as a stable reference for external integration.
    /// </remarks>
    public static EntityTypeBuilder<T> HasStandardKeys<T>(
        this EntityTypeBuilder<T> builder,
        Expression<Func<T, object?>> primaryKeyPropertySelector,
        Expression<Func<T, object?>> alternateKeyPropertySelector,
        string? alternateKeyColumnName = null,
        Action<PropertyBuilder<object?>>? configureAlternateKeyProperty = null)
        where T : class
    {
        builder.HasPrimaryKey(primaryKeyPropertySelector);
        builder.HasAlternateKey(alternateKeyPropertySelector, alternateKeyColumnName, configureAlternateKeyProperty);
        return builder;
    }
}