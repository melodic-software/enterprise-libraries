using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Callbacks.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Dispatching.Decoration;

public class EventDispatchingCallbackDecorator : DecoratorBase<IDispatchEvents>, IDispatchEvents
{
    private readonly IRaiseEventCallbacks _callbackService;
    private readonly ILogger<EventDispatchingCallbackDecorator> _logger;

    public EventDispatchingCallbackDecorator(IDispatchEvents decorated,
        IGetDecoratedInstance decoratorService,
        IRaiseEventCallbacks callbackService,
        ILogger<EventDispatchingCallbackDecorator> logger) : base(decorated, decoratorService)
    {
        _callbackService = callbackService;
        _logger = logger;
    }

    public async Task DispatchAsync(IEvent @event)
    {
        await Decorated.DispatchAsync(@event);
        RaiseCallbacks(@event);
    }

    private void RaiseCallbacks(IEvent @event)
    {
        _logger.LogDebug("Raising event callbacks.");
        _callbackService.RaiseCallbacks(@event);
        _logger.LogDebug("Event callbacks completed.");
    }
}
