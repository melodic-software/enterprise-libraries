using Enterprise.Caching.Abstractions;
using Enterprise.Caching.Options;
using Enterprise.Library.Core.Serialization.Abstract;
using Microsoft.Extensions.Caching.Distributed;

namespace Enterprise.Caching.Services;

public sealed class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IByteArraySerializer _byteArraySerializer;

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
        DistributedCacheEntryOptions cacheOptions = CacheOptions.CreateCacheEntryOptions(expiration);
        return _cache.SetAsync(cacheKey, bytes, cacheOptions, cancellationToken);
    }

    public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(cacheKey, cancellationToken);
    }
}
