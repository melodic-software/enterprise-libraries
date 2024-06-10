using Enterprise.DateTimes.Current;
using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DateTimes.Current.Abstract.Composite;
using Enterprise.DI.Core.Registration.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DateTimes.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // This is a service that was added with .NET 8.
        services.TryAddSingleton(TimeProvider.System);

        // These are composite interfaces. We only have two concrete implementations here.
        // We only want to register this type if no other registrations have been made.
        services.TryAddSingleton<ICurrentDateTimeProvider, CurrentDateTimeProvider>();
        services.TryAddSingleton<ICurrentDateTimeOffsetProvider, CurrentDateTimeOffsetProvider>();

        // With the application of the interface segregation principle, these smaller specific services may be requested.
        // Instead of creating separate instances of each, we are just delegating to the existing services that implement the composite interface.
        services.TryAddSingleton<IDateTimeNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeProvider>());
        services.TryAddSingleton<IDateTimeUtcNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeProvider>());
        services.TryAddSingleton<IDateTimeOffsetNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeOffsetProvider>());
        services.TryAddSingleton<IDateTimeOffsetUtcNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeOffsetProvider>());
    }
}
