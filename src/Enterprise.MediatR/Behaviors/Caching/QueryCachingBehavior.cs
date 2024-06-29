using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;
using Enterprise.Caching.Abstractions;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Caching;

internal sealed class QueryCachingBehavior<TRequest, TResult> : 
    IPipelineBehavior<TRequest, TResult>
    where TRequest : ICacheableQuery
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<QueryCachingBehavior<TRequest, TResult>> _logger;

    public QueryCachingBehavior(ICacheService cacheService, ILogger<QueryCachingBehavior<TRequest, TResult>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        // This is an example of the "cache aside" pattern.
        TResult? cachedResult = await _cacheService.GetAsync<TResult>(request.CacheKey, cancellationToken);

        string name = typeof(TRequest).Name;

        if (cachedResult is not null)
        {
            _logger.LogInformation("Cache hit for {Query}.", name);
            return cachedResult;
        }

        _logger.LogInformation("Cache miss for {Query}.", name);

        TResult result = await next();

        if (CanCache(result))
        {
            await _cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
        }
        else
        {
            _logger.LogWarning("Result is either null or a non successful result and cannot be cached.");
        }

        return result;
    }

    public bool CanCache(TResult result)
    {
        return result is Result { IsSuccess: true } || !Equals(result, default(TResult));
    }
}
