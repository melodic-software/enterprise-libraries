using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Dispatching;

public class EventDispatcher : EventDispatcherBase
{
    private readonly IResolveEventHandlers _eventHandlerResolver;

    public EventDispatcher(IGetDecoratedInstance decoratorService, IResolveEventHandlers eventHandlerResolver, ILogger<EventDispatcher> logger) : base(decoratorService, logger)
    {
        _eventHandlerResolver = eventHandlerResolver ?? throw new ArgumentNullException(nameof(eventHandlerResolver));
    }

    protected override Task<IEnumerable<IHandleEvent>> ResolveEventHandlers(IEvent @event)
    {
        return _eventHandlerResolver.ResolveAsync(@event);
    }
}
