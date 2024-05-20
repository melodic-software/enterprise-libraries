using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Enterprise.EntityFramework.AspNetCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Enterprise.EntityFramework.AspNetCore.Migration;

// TODO: Add configuration options (with defaults) to allow for configuring environment based restrictions, etc.

public static class DatabaseMigrationService
{
    /// <summary>
    /// This ensures that migrations must be run before the application has been deployed and can start up.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task EnsureNoPendingMigrationsAsync<T>(WebApplication app) where T : DbContext
    {
        if (app.Environment.IsDevelopment())
        {
            return;
        }

        DbContextResolutionResult<T>? dbContextResult = null;

        try
        {
            dbContextResult = await DbContextResolutionService.ResolveDbContextAsync<T>(app.Services);

            if (dbContextResult.DbContext == null)
            {
                throw new InvalidOperationException("DbContext could not be resolved.");
            }

            T dbContext = dbContextResult.DbContext;

            List<string> pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync()).ToList();

            if (!pendingMigrations.Any())
            {
                return;
            }

            app.Logger.LogWarning(
                "There are {PendingMigrationCount} migration(s) pending. " +
                "Migrations must be run separately before the app can start in deployed environments.",
                pendingMigrations.Count
            );

            throw new InvalidOperationException("Database is not fully migrated.");
        }
        finally
        {
            if (dbContextResult != null)
            {
                await dbContextResult.DisposeAsync();
            }
        }
    }

    public static async Task MigrateAsync<T>(WebApplication app) where T : DbContext
    {
        DbContextResolutionResult<T>? dbContextResult = null;

        try
        {
            if (!app.Environment.IsDevelopment())
            {
                app.Logger.LogInformation(
                    "Automatic Entity Framework migrations cannot be run in the current environment: {Environment}. " +
                    "They can only be run in the local development environment.",
                    app.Environment.EnvironmentName
                );

                return;
            }

            dbContextResult = await DbContextResolutionService.ResolveDbContextAsync<T>(app.Services);

            if (dbContextResult.DbContext == null)
            {
                throw new InvalidOperationException("DbContext could not be resolved.");
            }

            T dbContext = dbContextResult.DbContext;

            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while migrating the database.");
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
