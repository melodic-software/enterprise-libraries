using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Abstract.NonGeneric;
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
        using (_logger.BeginScope("Event: {@Event}", @event))
        {
            return _decoratedResolver.ResolveAsync(@event);
        }
    }

    public Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent
    {
        using (_logger.BeginScope("Event: {@Event}", @event))
        {
            return _decoratedResolver.ResolveAsync(@event);
        }
    }
}
