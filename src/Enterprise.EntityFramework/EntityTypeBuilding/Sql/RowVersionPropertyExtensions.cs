using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Enterprise.EntityFramework.Properties.PropertyNames;

namespace Enterprise.EntityFramework.EntityTypeBuilding.Sql;

public static class RowVersionPropertyExtensions
{
    public static EntityTypeBuilder<T> HasRowVersion<T>(this EntityTypeBuilder<T> builder, bool isRequired = true) where T : class
    {
        PropertyBuilder<byte[]> propertyBuilder = builder
            .Property<byte[]>(RowVersion)
            .IsRowVersion();

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return builder;
    }
}
