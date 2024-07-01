using Enterprise.Caching.Abstractions;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Library.Core.Serialization.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.Caching.Services;

internal sealed class CachingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Provide configuration around this registration.
        // What if the consuming app doesn't have a distributed cache implementation?

        services.TryAddSingleton(provider =>
        {
            // This will require a registration of IDistributedCache.
            IDistributedCache distributedCache = provider.GetRequiredService<IDistributedCache>();
            IByteArraySerializer byteArraySerializer = provider.GetRequiredService<IByteArraySerializer>();
            ICacheService cacheService = new CacheService(distributedCache, byteArraySerializer);
            return cacheService;
        });
    }
}
