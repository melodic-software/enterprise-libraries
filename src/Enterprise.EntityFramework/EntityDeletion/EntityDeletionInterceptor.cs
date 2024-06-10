using Enterprise.EntityFramework.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.EntityDeletion;

public class EntityDeletionInterceptor : SaveChangesInterceptor
{
    private readonly ILogger<EntityDeletionInterceptor> _logger;
    private readonly List<string> _propertyNamesToCheck;

    public EntityDeletionInterceptor(ILogger<EntityDeletionInterceptor> logger, List<string>? propertyNamesToCheck = null)
    {
        _logger = logger;
        _propertyNamesToCheck = propertyNamesToCheck ?? [PropertyNames.IsDeleted];
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PerformLogicalDeletion(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = new())
    {
        PerformLogicalDeletion(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void PerformLogicalDeletion(DbContextEventData eventData)
    {
        DbContext? context = eventData.Context;

        if (context == null)
        {
            return;
        }

        ChangeTracker changeTracker = context.ChangeTracker;

        var entriesToDelete = changeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        if (!entriesToDelete.Any())
        {
            return;
        }

        _logger.LogInformation("Entities to delete: {DeletionCount}", entriesToDelete.Count);
        entriesToDelete = entriesToDelete.Where(HasLogicalDeleteProperty).ToList();
        _logger.LogInformation("Entities to delete that have logical delete property: {DeletionCount}", entriesToDelete.Count);

        foreach (EntityEntry entry in entriesToDelete)
        {
            HandleLogicalDelete(entry);
        }

        _logger.LogInformation("Logical deletion intercept completed.");
    }

    private void HandleLogicalDelete(EntityEntry entry)
    {
        foreach (string propertyName in _propertyNamesToCheck)
        {
            IProperty? property = entry.Metadata.FindProperty(propertyName);

            if (property == null || entry.Property(propertyName).CurrentValue as bool? == true)
            {
                _logger.LogInformation("Entity does not contain a boolean \"{PropertyName}\" property or it's already set to true.", propertyName);
                continue;
            }
            
            _logger.LogInformation("Setting {PropertyName} to true.", propertyName);
            entry.Property(propertyName).CurrentValue = true;
        }

        _logger.LogInformation("Setting entity state to modified.");
        entry.State = EntityState.Modified;
    }

    private bool HasLogicalDeleteProperty(EntityEntry entry)
    {
        return _propertyNamesToCheck.Any(HaveProperty(entry));
    }

    private static Func<string, bool> HaveProperty(EntityEntry entry)
    {
        return propertyName =>
            entry.Metadata.FindProperty(propertyName) != null &&
            (
                entry.Property(propertyName).Metadata.ClrType == typeof(bool) ||
                entry.Property(propertyName).Metadata.ClrType == typeof(bool?)
            );
    }
}
