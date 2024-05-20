using Enterprise.EntityFramework.AspNetCore.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.Reset;

// TODO: Add configuration options (with defaults) to allow for configuring environment based restrictions, etc.

public static class DatabaseResetService
{
    public static async Task<bool> ResetDatabaseAsync<T>(WebApplication app) where T : DbContext
    {
        if (!app.Environment.IsDevelopment())
        {
            app.Logger.LogInformation(
                "The database cannot be reset in the current environment: {Environment}. " +
                "They can only be run in the local development environment.",
                app.Environment.EnvironmentName
            );

            return false;
        }

        DbContextResolutionResult<T>? dbContextResult = null;

        try
        {
            dbContextResult = await DbContextResolutionService.ResolveDbContextAsync<T>(app.Services);

            if (dbContextResult.DbContext == null)
            {
                throw new InvalidOperationException("Failed to resolve DbContext.");
            }

            T dbContext = dbContextResult.DbContext;

            app.Logger.LogInformation("Database reset starting...");

            await dbContext.Database.EnsureDeletedAsync();

            var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync()).ToList();

            if (pendingMigrations.Any())
            {
                // Apply migrations if there are any pending.
                app.Logger.LogInformation("Applying {PendingMigrationCount} migrations...", pendingMigrations.Count);
                await dbContext.Database.MigrateAsync();
            }
            else
            {
                // Just recreate the database if there are no migrations.
                // This uses the current state of the model.
                app.Logger.LogInformation("Recreating database...");
                await dbContext.Database.EnsureCreatedAsync();
            }

            app.Logger.LogInformation("Database reset complete.");

            return true;
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while resetting the database.");

            // The exception being thrown here was being swallowed even though we were throwing it up the stack.
            // We're actually going to stop and shutdown the application here.

            await app.StopAsync();
            await app.DisposeAsync();
            throw;
        }
        finally
        {
            if (dbContextResult != null)
            {
                await dbContextResult.DisposeAsync();
            }
        }
    }
}
