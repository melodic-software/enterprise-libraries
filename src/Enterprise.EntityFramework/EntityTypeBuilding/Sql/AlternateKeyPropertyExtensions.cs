﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql;

public static class AlternateKeyPropertyExtensions
{
    /// <summary>
    /// Constant for the default standard column name used for alternate keys.
    /// This value is used for local domain references, and is what external systems would reference.
    /// </summary>
    public const string DefaultAlternateKeyColumnName = "GlobalId";

    /// <summary>
    /// Configures the alternate key for a given entity, allowing for custom column naming and further configuration.
    /// </summary>
    /// <typeparam name="T">The entity type being configured.</typeparam>
    /// <param name="builder">The EntityTypeBuilder instance for the entity.</param>
    /// <param name="alternateKeyPropertySelector">Expression to select the alternate key property.</param>
    /// <param name="alternateKeyColumnName">Optional. Name of the column for the alternate key in the database.</param>
    /// <param name="configureAlternateKeyProperty">Optional. Action to further configure the alternate key property.</param>
    /// <remarks>
    /// This method sets the selected property as an alternate key and allows for custom column naming and additional configuration.
    /// The alternate key property will be configured to not generate values automatically in the database.
    /// </remarks>
    public static EntityTypeBuilder<T> HasAlternateKey<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object?>> alternateKeyPropertySelector,
        string? alternateKeyColumnName,
        Action<PropertyBuilder<object?>>? configureAlternateKeyProperty)
        where T : class
    {
        // Use the provided column name for the alternate key or the default if not specified.
        alternateKeyColumnName ??= DefaultAlternateKeyColumnName;

        // Configure the alternate key property with the specified column name.
        PropertyBuilder<object?> alternateKeyPropertyBuilder = builder
            .Property(alternateKeyPropertySelector)
            .HasColumnName(alternateKeyColumnName)
            .ValueGeneratedNever(); // Specifies that the value is generated by the application, not the database.

        // Allow for further customization.
        configureAlternateKeyProperty?.Invoke(alternateKeyPropertyBuilder);

        // Set the property as an alternate key and create a unique index for it.
        builder.HasAlternateKey(alternateKeyPropertySelector);

        return builder;
    }
}