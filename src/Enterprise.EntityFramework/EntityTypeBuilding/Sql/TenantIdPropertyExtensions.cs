using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static Enterprise.EntityFramework.Properties.PropertyNames;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql;

public static class TenantIdPropertyExtensions
{
    public static EntityTypeBuilder<T> HasTenantId<T>(this EntityTypeBuilder<T> builder, Func<string> getTenantId) where T : class
    {
        // TODO: Make this column type configurable?

        builder.Property<string>(TenantId)
            .HasColumnType("varchar(32)");

        builder.HasIndex(TenantId);

        builder.HasQueryFilter(entity => EF.Property<string>(entity, TenantId) == getTenantId());

        return builder;
    }
}