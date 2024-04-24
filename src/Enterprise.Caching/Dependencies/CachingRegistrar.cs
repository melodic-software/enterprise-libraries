using Enterprise.Caching.Abstractions;
using Enterprise.Caching.Services;
using Enterprise.DI.Core.Registration;
using Enterprise.Library.Core.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Caching.Dependencies;

internal class CachingRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider =>
        {
            // This will require a registration of IDistributedCache.
            IDistributedCache distributedCache = provider.GetRequiredService<IDistributedCache>();
            IByteArraySerializer byteArraySerializer = provider.GetRequiredService<IByteArraySerializer>();
            ICacheService cacheService = new CacheService(distributedCache, byteArraySerializer);
            return cacheService;
        });
    }
}