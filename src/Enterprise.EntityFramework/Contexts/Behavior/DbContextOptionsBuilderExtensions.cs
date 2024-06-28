using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.Contexts.Behavior;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureBehavior(this DbContextOptionsBuilder optionsBuilder)
    {
        // This sets the default query tracking behavior.
        // By default, the context instances will default to non-tracked queries.
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        // Lazy loading is not enabled by default, but you can add the call here.
    }

    private static void EnableLazyLoading(DbContextOptionsBuilder optionsBuilder)
    {
        // This is required for lazy loading.
        // NOTE: All navigational properties on a single entity must be marked virtual for this to work.
        optionsBuilder.UseLazyLoadingProxies();

        // Be careful with access to properties, one command / single property load is OK, but be careful of the following:
        // - Accessing the count method on a DbSet.
        // - Data binding a grid to lazy-loaded data results in sending N+1 commands to the database as each related record is loaded into a grid row.
        // - Lazy loading when no context is in scope.
    }
}
