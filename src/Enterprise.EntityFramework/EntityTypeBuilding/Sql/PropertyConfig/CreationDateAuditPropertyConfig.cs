using Enterprise.EntityFramework.Properties;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql.PropertyConfig;

/// <summary>
/// Configuration for creation date audit property.
/// </summary>
public record CreationDateAuditPropertyConfig : AuditDatePropertyConfig
{
    public override string PropertyName => PropertyNames.CreationDate;

    public CreationDateAuditPropertyConfig(string? defaultSql = null, bool applyDefaultToNullable = false, bool isShadowProperty = false)
    {
        DefaultSql = defaultSql;
        ApplyDefaultToNullable = applyDefaultToNullable;
        IsShadowProperty = isShadowProperty;
        IsNullable = false;
    }
}