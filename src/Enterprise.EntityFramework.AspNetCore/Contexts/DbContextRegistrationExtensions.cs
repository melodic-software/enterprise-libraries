using Enterprise.EntityFramework.AspNetCore.Interceptors;
using Enterprise.EntityFramework.Contexts.Behavior;
using Enterprise.EntityFramework.Contexts.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.EntityFramework.AspNetCore.Contexts;

public static class DbContextRegistrationExtensions
{
    public static void RegisterDbContext<T>(this IServiceCollection services, Action<DbContextOptionsBuilder>? configure = null) 
        where T : DbContext
    {
        // The default lifetime is "scoped" (one instance per HTTP request).
        // Pooling can help with performance where there are many short-lived transactions.
        // This also pools connection and other database resources.
        // Using "AddDbContext" will result in new context instances created for every request to the API.
        // https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics
        // https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Performance/AspNetContextPoolingWithState/Program.cs
        // https://dev.to/mohammadkarimi/how-dbcontext-pooling-works-4goc

        services.AddDbContextPool<T>((serviceProvider, builder) =>
        {
            IHostEnvironment environment = serviceProvider.GetRequiredService<IHostEnvironment>();

            configure?.Invoke(builder);

            builder.ConfigureLogging(environment);
            builder.ConfigureBehavior();
            builder.ConfigureInterceptors(serviceProvider);
        });
    }
}
