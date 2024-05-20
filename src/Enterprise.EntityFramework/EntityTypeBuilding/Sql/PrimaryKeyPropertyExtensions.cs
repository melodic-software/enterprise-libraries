using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql;

public static class PrimaryKeyPropertyExtensions
{
    /// <summary>
    /// Configures the primary key for a given entity, using either a property selector or a property name.
    /// </summary>
    /// <typeparam name="T">The entity type being configured.</typeparam>
    /// <param name="builder">The EntityTypeBuilder instance for the entity.</param>
    /// <param name="primaryKeyPropertySelector">Expression to select the primary key property.</param>
    /// <param name="primaryKeyPropertyName">Name of the shadow primary key property.</param>
    /// <remarks>
    /// If a property name is provided, it configures a shadow property as the primary key. If a selector is provided, it uses the selected property.
    /// The primary key property will be configured with auto-incrementing behavior.
    /// </remarks>
    public static EntityTypeBuilder<T> HasPrimaryKey<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object?>>? primaryKeyPropertySelector,
        string? primaryKeyPropertyName = null) where T : class
    {
        HandleModelProperty(builder, primaryKeyPropertySelector, primaryKeyPropertyName);
        HandleShadowProperty(builder, primaryKeyPropertyName);
        return builder;
    }

    private static void HandleModelProperty<T>(EntityTypeBuilder<T> builder, Expression<Func<T, object?>>? primaryKeyPropertySelector,
        string? primaryKeyPropertyName = null) where T : class
    {
        if (!string.IsNullOrWhiteSpace(primaryKeyPropertyName) || primaryKeyPropertySelector == null)
        {
            return;
        }

        // Configure the primary key property with auto-increment.
        builder.Property(primaryKeyPropertySelector)
            .ValueGeneratedOnAdd();

        // Set the property as the primary key.
        builder.HasKey(primaryKeyPropertySelector);
    }

    private static void HandleShadowProperty<T>(EntityTypeBuilder<T> builder, string? primaryKeyPropertyName = null)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(primaryKeyPropertyName))
        {
            return;
        }

        // Configure the primary key property with auto-increment.
        builder.Property<int>(primaryKeyPropertyName)
            .ValueGeneratedOnAdd();

        // Set the shadow property as the primary key.
        builder.HasKey(primaryKeyPropertyName);
    }
}
