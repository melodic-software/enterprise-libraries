using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.ServiceProviders;

/// <summary>
/// Provides methods for ensuring that the appropriate service provider scope is used.
/// </summary>
public static class ScopedProviderService
{
    /// <summary>
    /// Returns an appropriate scoped <see cref="IServiceProvider"/>.
    /// If the current service provider is already scoped, it is returned.
    /// Otherwise, a new scope is created.
    /// </summary>
    /// <param name="serviceProvider">The current service provider.</param>
    /// <returns>An <see cref="IServiceProvider"/> that is scoped appropriately.</returns>
    public static IServiceProvider GetScopedProvider(IServiceProvider serviceProvider)
    {
        // Check if the current provider is already a scoped service provider.
        if (serviceProvider.GetService<IServiceScopeFactory>() == null)
        {
            return serviceProvider;
        }

        if (serviceProvider is IServiceScope)
        {
            return serviceProvider;
        }

        // Create a new scope if we are in the root scope.
        IServiceScopeFactory scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        IServiceScope scope = scopeFactory.CreateScope();
        return scope.ServiceProvider;
    }
}
