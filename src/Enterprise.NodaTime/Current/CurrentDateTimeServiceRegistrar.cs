using Enterprise.DateTimes.Current.Abstract.Composite;
using Enterprise.DI.Core.Registration.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.NodaTime.Current;

internal sealed class CurrentDateTimeServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Remove all existing registrations.
        services.RemoveAll<ICurrentDateTimeProvider>();
        services.RemoveAll<ICurrentDateTimeOffsetProvider>();

        // Add NodaTime implementations.
        services.TryAddSingleton<ICurrentDateTimeProvider>(provider => new CurrentDateTimeProvider());
        services.TryAddSingleton<ICurrentDateTimeOffsetProvider>(provider => new CurrentDateTimeOffsetProvider());
    }
}
