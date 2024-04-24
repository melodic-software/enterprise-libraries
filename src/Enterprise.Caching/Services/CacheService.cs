using Enterprise.Caching.Abstractions;
using Enterprise.Library.Core.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;

namespace Enterprise.Caching.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IByteArraySerializer _byteArraySerializer;

    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
    };

    public CacheService(IDistributedCache cache, IByteArraySerializer byteArraySerializer)
    {
        _cache = cache;
        _byteArraySerializer = byteArraySerializer;
    }

    public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await _cache.GetAsync(cacheKey, cancellationToken);
        T? value = bytes is null ? default : _byteArraySerializer.Deserialize<T>(bytes);
        return value;
    }

    public Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        byte[] bytes = _byteArraySerializer.Serialize(value);
        DistributedCacheEntryOptions cacheOptions = CreateCacheEntryOptions(expiration);
        return _cache.SetAsync(cacheKey, bytes, cacheOptions, cancellationToken);
    }

    public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(cacheKey, cancellationToken);
    }

    private DistributedCacheEntryOptions CreateCacheEntryOptions(TimeSpan? expiration)
    {
        DistributedCacheEntryOptions options = expiration is not null
            ? new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration }
            : DefaultExpiration;

        return options;
    }
}