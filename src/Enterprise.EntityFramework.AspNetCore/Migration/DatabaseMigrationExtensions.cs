using Enterprise.EntityFramework.AspNetCore.Migration.Environments;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.AspNetCore.Migration;

public static class DatabaseMigrationExtensions
{
    /// <summary>
    /// Apply migrations for the provided EF database context.
    /// The migration strategy will depend on the host environment.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="app"></param>
    /// <param name="seedAction"></param>
    /// <returns></returns>
    public static async Task MigrateAsync<T>(this WebApplication app, Func<T, Task>? seedAction = null) where T : DbContext
    {
        await DatabaseMigrationService.EnsureNoPendingMigrationsAsync<T>(app);
        await LocalDevEnvironmentMigrationService.MigrateLocalDevEnvironmentAsync(app, seedAction);
        await PreProdEnvironmentMigrationService.MigratePreProdEnvironmentAsync<T>(app);
    }
}
