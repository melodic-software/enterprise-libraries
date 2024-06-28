using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.Contexts.Factory;

// NOTE: This isn't being used if a call to AddPooledDbContextFactory has not been made for this db context.
// This is really for demonstration purposes. For ASP.NET Core web APIs, it is preferred to use the AddDbContextPool<T> if needed.

// https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-di%2Cexpression-api-with-constant#dbcontext-pooling
// https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Performance/AspNetContextPoolingWithState

public class DbContextFactory<T> : IDbContextFactory<T> where T :DbContext
{
    private readonly IDbContextFactory<T> _pooledFactory;
    private readonly Action<T>? _reassignState;

    public DbContextFactory(
        IDbContextFactory<T> pooledFactory,
        Action<T>? reassignState = null)
    {
        _pooledFactory = pooledFactory;
        _reassignState = reassignState;
    }

    public T CreateDbContext()
    {
        T dbContext = _pooledFactory.CreateDbContext();

        // When using context pooling, there are a few things that we need to look out for.
        // The instances themselves are only created once, which means we have to take care with DbContext constructor parameters.
        // The common approach here is to refactor those to properties, which are reset after a context is returned to the pool.
        // We can inject scoped instances of dependencies here and assign them to properties on the context instance.

        _reassignState?.Invoke(dbContext);

        return dbContext;
    }
}
