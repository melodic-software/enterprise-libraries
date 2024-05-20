using Enterprise.EntityFramework.EntityTypeBuilding.Sql.PropertyConfig;
using Enterprise.EntityFramework.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql;

/// <summary>
/// Extension methods for EntityTypeBuilder to add and configure audit properties.
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Adds and configures audit shadow properties for creation and modification dates.
    /// </summary>
    public static EntityTypeBuilder AddAuditDateShadowProperties(
        this EntityTypeBuilder entityTypeBuilder,
        CreationDateAuditPropertyConfig? creationDateConfig = null,
        DateModifiedPropertyConfig? modifiedDateConfig = null)
    {
        creationDateConfig ??= new CreationDateAuditPropertyConfig(isShadowProperty: true);
        modifiedDateConfig ??= new DateModifiedPropertyConfig(isShadowProperty: true);

        entityTypeBuilder.HasCreationDateProperty(creationDateConfig);
        entityTypeBuilder.HasDateModifiedProperty(modifiedDateConfig);

        return entityTypeBuilder;
    }

    public static EntityTypeBuilder HasCreationDateProperty(this EntityTypeBuilder entityTypeBuilder, CreationDateAuditPropertyConfig? configuration = null)
    {
        configuration ??= new CreationDateAuditPropertyConfig();
        return ConfigureAuditDateProperty(entityTypeBuilder, configuration);
    }

    public static EntityTypeBuilder HasDateModifiedProperty(this EntityTypeBuilder entityTypeBuilder, DateModifiedPropertyConfig? configuration = null)
    {
        configuration ??= new DateModifiedPropertyConfig();
        return ConfigureAuditDateProperty(entityTypeBuilder, configuration);
    }

    private static EntityTypeBuilder ConfigureAuditDateProperty(EntityTypeBuilder entityTypeBuilder, AuditDatePropertyConfig configuration)
    {
        ConfigureShadowProperty(entityTypeBuilder, configuration);
        ConfigureRegularProperty(entityTypeBuilder, configuration);
        return entityTypeBuilder;
    }

    private static void ConfigureShadowProperty(EntityTypeBuilder entityTypeBuilder, AuditDatePropertyConfig configuration)
    {
        if (!configuration.IsShadowProperty)
        {
            return;
        }

        Type type = configuration.IsNullable ? typeof(DateTime?) : typeof(DateTime);

        Configure(configuration, entityTypeBuilder, type);
    }

    public static void ConfigureRegularProperty(EntityTypeBuilder entityTypeBuilder, AuditDatePropertyConfig configuration)
    {
        if (configuration.IsShadowProperty)
        {
            return;
        }

        Type entityType = entityTypeBuilder.Metadata.ClrType;
        PropertyInfo? propertyInfo = entityType.GetProperty(configuration.PropertyName, BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null || propertyInfo.PropertyType != typeof(DateTime) && propertyInfo.PropertyType != typeof(DateTime?))
        {
            throw new InvalidOperationException($"Entity does not have a property '{configuration.PropertyName}' of correct type.");
        }

        Configure(configuration, entityTypeBuilder, propertyInfo.PropertyType);
    }

    private static void Configure(AuditDatePropertyConfig configuration, EntityTypeBuilder entityTypeBuilder, Type type)
    {
        PropertyBuilder propertyBuilder = entityTypeBuilder.Property(type, configuration.PropertyName);

        // Check if the type is nullable DateTime (DateTime?).
        bool isNullableDateTime = type == typeof(DateTime?) || 
                                  Nullable.GetUnderlyingType(type) == typeof(DateTime);

        if (!isNullableDateTime)
        {
            propertyBuilder.IsRequired();
        }

        if (configuration.DefaultSql != null)
        {
            propertyBuilder.HasDefaultValueSql(configuration.DefaultSql);
        }
        else if (!isNullableDateTime || configuration.ApplyDefaultToNullable)
        {
            propertyBuilder.HasValueGenerator<UtcNowValueGenerator>();
        }
    }
}
