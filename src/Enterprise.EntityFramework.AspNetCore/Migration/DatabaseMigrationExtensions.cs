using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.AspNetCore.Migration;

public static class DatabaseMigrationExtensions
{
    public static async Task EnsureNoPendingMigrationsAsync<T>(this WebApplication app) where T : DbContext
    {
        await DatabaseMigrationService.EnsureNoPendingMigrationsAsync<T>(app);
    }

    public static async Task MigrateAsync<T>(WebApplication app) where T : DbContext
    {
        await DatabaseMigrationService.MigrateAsync<T>(app);
    }
}
