using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static Enterprise.EntityFramework.Properties.PropertyNames;

namespace Enterprise.EntityFramework.Properties.Shadow.Audit;

public static class ShadowPropertyExtensions
{
    public static void UpdateAuditShadowProperties(this DbContext dbContext)
    {
        foreach (EntityEntry entry in dbContext.ChangeTracker.Entries())
        {
            DateTimeOffset now = TimeProvider.System.GetUtcNow();
            entry.UpdateShadowProperty(dbContext, EntityState.Added, CreationDate, now.DateTime);
            entry.UpdateShadowProperty(dbContext, EntityState.Modified, DateModified, now.DateTime);
        }
    }

    public static void UpdateShadowProperty(this EntityEntry entry, DbContext dbContext, EntityState entityState, string propertyName, object value)
    {
        if (entry.State != entityState || entry.Properties.All(x => x.Metadata.Name != propertyName))
        {
            return;
        }

        object? currentDateModifiedValue = dbContext.GetShadowPropertyValue(entry, propertyName);
        PropertyEntry dateModifiedPropertyEntry = entry.Property(propertyName);
        dateModifiedPropertyEntry.CurrentValue ??= value;
    }
}
