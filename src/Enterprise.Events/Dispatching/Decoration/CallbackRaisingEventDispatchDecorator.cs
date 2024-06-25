using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Dispatching.Decoration;

/// <summary>
/// This decorator ensures that callbacks are raised after events have been dispatched.
/// </summary>
public class CallbackRaisingEventDispatchDecorator : DecoratorBase<IDispatchEvents>, IDispatchEvents
{
    private readonly IRaiseEventCallbacks _callbackService;

    public CallbackRaisingEventDispatchDecorator(IDispatchEvents decorated,
        IGetDecoratedInstance decoratorService,
        IRaiseEventCallbacks callbackService) : base(decorated, decoratorService)
    {
        _callbackService = callbackService;
    }

    public async Task DispatchAsync(IEvent[] events, CancellationToken cancellationToken = default)
    {
        foreach (IEvent @event in events)
        {
            await DispatchAsync(@event, cancellationToken); 
        }
    }

    public async Task DispatchAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        await Decorated.DispatchAsync(@event, cancellationToken);
        RaiseCallbacks(@event);
    }

    private void RaiseCallbacks(IEvent @event)
    {
        _callbackService.RaiseCallbacks(@event);
    }
}
