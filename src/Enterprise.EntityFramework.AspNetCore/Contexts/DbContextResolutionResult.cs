using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.EntityFramework.AspNetCore.Contexts;

public class DbContextResolutionResult<T> : IAsyncDisposable where T : DbContext
{
    private AsyncServiceScope? ServiceScope { get; }

    public T? DbContext { get; }

    public DbContextResolutionResult(T? dbContext, AsyncServiceScope? serviceScope = null)
    {
        DbContext = dbContext;
        ServiceScope = serviceScope;
    }

    public async ValueTask DisposeAsync()
    {
        if (DbContext != null)
            await DbContext.DisposeAsync();

        if (ServiceScope.HasValue)
            await ServiceScope.Value.DisposeAsync();
    }
}
