using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace Enterprise.EntityFramework.Providers.Sqlite;

public static class SqliteDateTimeOffsetSortFixer
{
    public static void AllowDateTimeOffsetSortingForSqlite(this DbContext dbContext, ModelBuilder modelBuilder)
    {
        if (dbContext.Database.ProviderName != ProviderConstants.SqlLiteProviderName)
            return;

        // This is a fix to allow sorting on DateTimeOffset when using Sqlite.
        // It is based on: https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/

        // SQLite does not have proper support for DateTimeOffset via Entity Framework Core.
        // See the limitations here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations

        // To work around this, when the Sqlite database provider is used,
        // all model properties of type DateTimeOffset use the DateTimeOffsetToBinaryConverter
        // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754 

        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            List<PropertyInfo> dateTimeOffsetProperties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?))
                .ToList();

            foreach (PropertyInfo property in dateTimeOffsetProperties)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(new DateTimeOffsetToBinaryConverter());
            }
        }
    }
}