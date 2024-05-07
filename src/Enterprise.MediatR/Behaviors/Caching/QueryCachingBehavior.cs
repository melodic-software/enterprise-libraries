using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.Caching.Abstractions;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Behaviors.Caching;

internal sealed class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : Result
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;

    public QueryCachingBehavior(ICacheService cacheService, ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // This is an example of the "cache aside" pattern.

        TResponse? cachedResult = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

        string name = typeof(TRequest).Name;

        if (cachedResult is not null)
        {
            _logger.LogInformation("Cache hit for {Query}", name);
            return cachedResult;
        }

        _logger.LogInformation("Cache miss for {Query}", name);

        TResponse result = await next();

        if (result.IsSuccess)
        {
            await _cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
        }

        return result;
    }
}
