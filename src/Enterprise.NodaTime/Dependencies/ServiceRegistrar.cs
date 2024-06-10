using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DateTimes.Current.Abstract.Composite;
using Enterprise.DateTimes.Utc;
using Enterprise.DI.Core.Registration;
using Enterprise.NodaTime.Current;
using Enterprise.NodaTime.Utc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.NodaTime.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        ReplaceRegistrations(services);

        // With the application of the interface segregation principle, these smaller specific services may be requested.
        // Instead of creating separate instances of each, we are just delegating to the existing services that implement the composite interface.
        // These may already be registered, but we can safely use the TryAdd to prevent duplication.
        services.TryAddSingleton<IDateTimeNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeProvider>());
        services.TryAddSingleton<IDateTimeUtcNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeProvider>());
        services.TryAddSingleton<IDateTimeOffsetNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeOffsetProvider>());
        services.TryAddSingleton<IDateTimeOffsetUtcNowProvider>(provider => provider.GetRequiredService<ICurrentDateTimeOffsetProvider>());
    }

    private static void ReplaceRegistrations(IServiceCollection services)
    {
        // Replace base registrations (if they exist) with these.

        // TODO: Update the enterprise DI package with an extension method that simplifies this replacement syntax.
        // TODO: Consider adding logging support, so it is clear what has been registered and when.

        services.Replace(
            ServiceDescriptor.Describe(
                typeof(ICurrentDateTimeProvider),
                typeof(CurrentDateTimeProvider),
                ServiceLifetime.Singleton
            )
        );

        services.Replace(
            ServiceDescriptor.Describe(
                typeof(ICurrentDateTimeOffsetProvider),
                typeof(CurrentDateTimeOffsetProvider),
                ServiceLifetime.Singleton)
        );

        services.Replace(
            ServiceDescriptor.Describe(
                typeof(IEnsureUtcService),
                typeof(EnsureUtcService),
                ServiceLifetime.Singleton)
        );
    }
}
