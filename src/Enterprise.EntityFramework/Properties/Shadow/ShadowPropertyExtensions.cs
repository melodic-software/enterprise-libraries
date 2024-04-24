using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;

namespace Enterprise.EntityFramework.Properties.Shadow;

public static class ShadowPropertyExtensions
{
    public static T? GetShadowPropertyValue<T>(this DbContext dbContext, object entity, string propertyName) where T : IConvertible
    {
        object? value = dbContext.GetShadowPropertyValue(entity, propertyName);

        T? result = value != null ?
            (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture) :
            default;

        return result;
    }

    public static object? GetShadowPropertyValue(this DbContext dbContext, object entity, string propertyName)
    {
        object? value;

        if (entity is EntityEntry entry)
        {
            value = entry.Property(propertyName).CurrentValue;
        }
        else
        {
            value = dbContext.Entry(entity).Property(propertyName).CurrentValue;
        }

        return value;
    }
}