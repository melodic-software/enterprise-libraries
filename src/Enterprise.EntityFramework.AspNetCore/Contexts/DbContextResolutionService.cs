using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.EntityFramework.AspNetCore.Contexts;

public static class DbContextResolutionService
{
    public static async Task<DbContextResolutionResult<T>> ResolveDbContextAsync<T>(IServiceProvider services) where T : DbContext
    {
        IServiceScopeFactory? serviceScopeFactory = services.GetService<IServiceScopeFactory>();
        AsyncServiceScope? scope = serviceScopeFactory?.CreateAsyncScope();

        if (!scope.HasValue)
        {
            return new DbContextResolutionResult<T>(null, scope);
        }

        DbContextResolutionResult<T> result = await GetResultAsync<T>(scope.Value);

        return result;
    }

    public static async Task<DbContextResolutionResult<T>> ResolveDbContextAsync<T>(IApplicationBuilder app) where T : DbContext
    {
        await using AsyncServiceScope scope = app.ApplicationServices.CreateAsyncScope();

        DbContextResolutionResult<T> result = await GetResultAsync<T>(scope);

        return result;
    }

    private static async Task<DbContextResolutionResult<T>> GetResultAsync<T>(AsyncServiceScope scope) where T : DbContext
    {
        T? dbContext = scope.ServiceProvider.GetService<T>();

        if (dbContext != null)
        {
            return new DbContextResolutionResult<T>(dbContext, scope);
        }

        // If a direct instance has not been registered, we can attempt to resolve via a factory.
        IDbContextFactory<T>? factory = scope.ServiceProvider.GetService<IDbContextFactory<T>>();

        if (factory != null)
        {
            dbContext = await factory.CreateDbContextAsync();
        }

        return new DbContextResolutionResult<T>(dbContext, scope);
    }
}
