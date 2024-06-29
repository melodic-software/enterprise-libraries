using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Enterprise.EntityFramework.AspNetCore.Migration.Environments;

public static class PreProdEnvironmentMigrationService
{
    /// <summary>
    /// Configures deployed non-production environments.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task MigratePreProdEnvironmentAsync<T>(WebApplication app) where T : DbContext
    {
        // We handle local development configuration separately and do NOT run migrations automatically in production.
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            return;
        }

        // We can apply migrations at runtime.
        // https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying#apply-migrations-at-runtime
        await DatabaseMigrationService.MigrateAsync<T>(app);
    }
}
