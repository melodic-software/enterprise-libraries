using Enterprise.DI.Core.ServiceCollection.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.DI.Core.ServiceCollection;

/// <summary>
/// Extension methods for registering the <see cref="ServiceDescriptorRegistry"/> in the service collection.
/// </summary>
public static class ServiceDescriptorRegistryExtensions
{
    /// <summary>
    /// Registers the <see cref="ServiceDescriptorRegistry"/> as a singleton service.
    /// This method should be called right before the application is built to capture the final state of the service collection.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    public static void AddServiceDescriptorRegistry(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IServiceDescriptorRegistry>(provider =>
        {
            IServiceCollection services = builder.Services;
            return new ServiceDescriptorRegistry(services);
        });
    }
}
