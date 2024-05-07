using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Handlers.Resolution.Decoration;

public class LoggingEventHandlerResolver : IResolveEventHandlers
{
    private readonly IResolveEventHandlers _decoratedResolver;
    private readonly ILogger<LoggingEventHandlerResolver> _logger;

    public LoggingEventHandlerResolver(IResolveEventHandlers decoratedResolver, ILogger<LoggingEventHandlerResolver> logger)
    {
        _decoratedResolver = decoratedResolver;
        _logger = logger;
    }

    public Task<IEnumerable<IHandleEvent>> ResolveAsync(IEvent @event)
    {
        Type eventType = @event.GetType();

        using (_logger.BeginScope("Event: {EventType}", eventType.Name))
        {
            return _decoratedResolver.ResolveAsync(@event);
        }
    }

    public Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent
    {
        Type eventType = typeof(T);

        using (_logger.BeginScope("Event: {EventType}", eventType.Name))
        {
            return _decoratedResolver.ResolveAsync(@event);
        }
    }
}
