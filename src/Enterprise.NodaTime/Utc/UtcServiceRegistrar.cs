using Enterprise.DateTimes.Utc;
using Enterprise.DI.Core.Registration.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.NodaTime.Utc;

internal sealed class UtcServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Remove all existing registrations.
        services.RemoveAll<IEnsureUtcService>();

        // Add NodaTime implementations.
        services.TryAddSingleton<IEnsureUtcService>(provider => new EnsureUtcService());
    }
}
