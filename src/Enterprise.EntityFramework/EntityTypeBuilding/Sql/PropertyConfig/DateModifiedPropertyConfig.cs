using Enterprise.EntityFramework.Properties;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql.PropertyConfig;

/// <summary>
/// Configuration for date modified audit property.
/// </summary>
public record DateModifiedPropertyConfig : AuditDatePropertyConfig
{
    public override string PropertyName => PropertyNames.DateModified;

    public DateModifiedPropertyConfig(string? defaultSql = null, bool applyDefaultToNullable = false, bool isShadowProperty = false)
    {
        DefaultSql = defaultSql;
        ApplyDefaultToNullable = applyDefaultToNullable;
        IsShadowProperty = isShadowProperty;
        IsNullable = true;
    }
}