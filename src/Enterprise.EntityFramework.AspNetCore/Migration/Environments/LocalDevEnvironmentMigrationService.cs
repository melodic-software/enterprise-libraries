using Enterprise.EntityFramework.AspNetCore.Reset;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Enterprise.EntityFramework.Config.ConfigKeyConstants;

namespace Enterprise.EntityFramework.AspNetCore.Migration.Environments;

public static class LocalDevEnvironmentMigrationService
{
    public static async Task MigrateLocalDevEnvironmentAsync<T>(WebApplication app, Func<T, Task>? seedAction = null) where T : DbContext
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        bool resetDatabase = app.Configuration.GetValue(ResetDatabaseConfigKeyName, false);

        if (resetDatabase)
        {
            // This deletes and recreates the database on startup, so we can begin with a clean slate.
            // If migrations have not been added, the initial schema is based off the current state of the model.
            bool databaseReset = await DatabaseResetService.ResetDatabaseAsync<T>(app);

            if (databaseReset)
            {
                await SeedLocalDataAsync(app, seedAction);
            }
        }
        else
        {
            // We can apply migrations at runtime.
            // https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying#apply-migrations-at-runtime
            await DatabaseMigrationService.MigrateAsync<T>(app);
        }
    }

    private static async Task SeedLocalDataAsync<T>(WebApplication app, Func<T, Task>? seedAction) where T : DbContext
    {
        IServiceScopeFactory serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();

        T dbContext = scope.ServiceProvider.GetRequiredService<T>();

        if (seedAction != null)
        {
            app.Logger.LogInformation("Seeding data...");
            await seedAction(dbContext);
            app.Logger.LogInformation("Data seeding complete.");
        }

        await dbContext.SaveChangesAsync();
    }

}
