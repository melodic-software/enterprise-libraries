using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.AspNetCore.Contexts;

public static class DbContextResolutionExtensions
{
    public static async Task<DbContextResolutionResult<T>> ResolveDbContextAsync<T>(this IServiceProvider services) where T : DbContext
    {
        return await DbContextResolutionService.ResolveDbContextAsync<T>(services);
    }

    public static async Task<DbContextResolutionResult<T>> ResolveDbContextAsync<T>(this IApplicationBuilder app) where T : DbContext
    {
        return await DbContextResolutionService.ResolveDbContextAsync<T>(app);
    }
}
