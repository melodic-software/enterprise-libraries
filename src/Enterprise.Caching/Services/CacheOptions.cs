using Microsoft.Extensions.Caching.Distributed;

namespace Enterprise.Caching.Services;

public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
    };

    public static DistributedCacheEntryOptions CreateCacheEntryOptions(TimeSpan? expiration)
    {
        DistributedCacheEntryOptions options = expiration is not null
            ? new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration }
            : DefaultExpiration;

        return options;
    }
}
