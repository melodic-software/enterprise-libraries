using Enterprise.EntityFramework.AspNetCore.Concurrency;
using Enterprise.EntityFramework.EntityDeletion;
using Enterprise.EntityFramework.Interceptors.Demo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.EntityFramework.AspNetCore.Interceptors;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureInterceptors(this DbContextOptionsBuilder optionsBuilder, IServiceProvider provider)
    {
        IServiceScopeFactory scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        using IServiceScope scope = scopeFactory.CreateScope();

        // https://www.milanjovanovic.tech/blog/how-to-use-ef-core-interceptors

        optionsBuilder.AddInterceptors(
            scope.ServiceProvider.GetRequiredService<ConcurrencyErrorHandlingInterceptor>(),
            scope.ServiceProvider.GetRequiredService<EntityDeletionInterceptor>(),
            //scope.ServiceProvider.GetRequiredService<DomainEventQueuingInterceptor>(),
            scope.ServiceProvider.GetRequiredService<CustomDbCommandInterceptor>()
        );
    }
}
