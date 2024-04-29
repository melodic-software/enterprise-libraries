using System.Linq.Expressions;
using Enterprise.EntityFramework.Properties;
using Enterprise.Multitenancy.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

// NOTE: This didn't work well with db context pooling.

namespace Enterprise.EntityFramework.AspNetCore.MultiTenancy;

internal static class MultiTenancyDbContextExtensions
{
    internal static void ConfigureMultiTenancy(this ModelBuilder modelBuilder, MultiTenancyParameters parameters)
    {
        if (!parameters.MultiTenancyEnabled)
        {
            parameters.Logger?.LogInformation("Multi-tenancy is not enabled.");
            return;
        }

        IGetTenantId tenantIdService = parameters.TenantIdService ??
                                       throw new InvalidOperationException($"An instance of {nameof(IGetTenantId)} is required to configure multi tenancy.");

        List<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes().ToList();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            Type clrType = entityType.ClrType;

            // Skip if the type is not a class, is abstract, or is in the list of excluded entity types.
            // Depending on the configuration, many-to-many relationships may be represented as Dictionary<string, object>.
            // It is considered a shared type and those cannot be used here either.
            bool typeIsExcluded = !clrType.IsClass || clrType.IsAbstract ||
                                  parameters.ExcludedEntityTypes.Contains(clrType) ||
                                  clrType == typeof(Dictionary<string, object>);

            if (typeIsExcluded)
            {
                parameters.Logger?.LogInformation(
                    "Entity type excluded: {EntityType}. " +
                    "Shadow property \"{TenantId}\" and global query filter will not be added.",
                    entityType.Name, PropertyNames.TenantId
                );

                continue;
            }

            parameters.Logger?.LogInformation(
                "Adding shadow property \"{TenantId}\", and global query filter for entity type: {EntityType}.",
                PropertyNames.TenantId, entityType.Name
            );

            modelBuilder.Entity(entityType.ClrType)
                .Property<string>(PropertyNames.TenantId)
                .IsRequired();

            modelBuilder.Entity(entityType.ClrType)
                .HasQueryFilter(CreateTenantFilterExpression(entityType.ClrType, tenantIdService.GetTenantId()));
        }
    }

    private static LambdaExpression CreateTenantFilterExpression(Type entityType, string? tenantId)
    {
        tenantId ??= string.Empty;

        ParameterExpression parameterExp = Expression.Parameter(entityType, "e");
        MemberExpression propertyExp = Expression.PropertyOrField(parameterExp, PropertyNames.TenantId);
        ConstantExpression tenantIdExp = Expression.Constant(tenantId);
        BinaryExpression equalExp = Expression.Equal(propertyExp, tenantIdExp);

        return Expression.Lambda(equalExp, parameterExp);
    }
}