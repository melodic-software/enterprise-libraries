using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Handlers.Resolution.Decoration;

public class CachingEventHandlerResolver : IResolveEventHandlers
{
    private readonly IResolveEventHandlers _decoratedResolver;
    private readonly ILogger<CachingEventHandlerResolver> _logger;

    private readonly Dictionary<Type, Task<IEnumerable<IHandleEvent>>> _nonGenericHandlerCache = [];
    private readonly Dictionary<Type, object> _genericHandlerCache = new();

    public CachingEventHandlerResolver(IResolveEventHandlers decoratedResolver, ILogger<CachingEventHandlerResolver> logger)
    {
        _decoratedResolver = decoratedResolver ?? throw new ArgumentNullException(nameof(decoratedResolver));
        _logger = logger;
    }

    public Task<IEnumerable<IHandleEvent>> ResolveAsync(IEvent @event)
    {
        Type eventType = @event.GetType();

        if (_nonGenericHandlerCache.TryGetValue(eventType, out Task<IEnumerable<IHandleEvent>>? handlersTask))
        {
            _logger.LogDebug("Event handler resolution task found in cache.");
            return handlersTask;
        }

        _logger.LogDebug("Event handler resolution task not found in cache.");

        handlersTask = _decoratedResolver.ResolveAsync(@event);
        _nonGenericHandlerCache[eventType] = handlersTask;

        _logger.LogDebug("Event handler resolution task added to cache.");

        return handlersTask;
    }

    public Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent
    {
        Type eventType = typeof(T);

        if (_genericHandlerCache.TryGetValue(eventType, out object? cachedHandlers))
        {
            _logger.LogDebug("Event handler resolution task found in generic cache.");
            return (Task<IEnumerable<IHandleEvent<T>>>)cachedHandlers;
        }

        _logger.LogDebug("Event handler resolution task not found in generic cache.");

        Task<IEnumerable<IHandleEvent<T>>> handlersTask = _decoratedResolver.ResolveAsync(@event);

        _genericHandlerCache[eventType] = handlersTask;

        _logger.LogDebug("Event handler resolution task added to generic cache.");

        return handlersTask;
    }
}
