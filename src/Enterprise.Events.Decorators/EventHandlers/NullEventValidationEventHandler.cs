using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Handlers;
using Enterprise.Events.Services.Handlers.Decoration;

namespace Enterprise.Events.Decorators.EventHandlers;

public class NullEventValidationEventHandler<T> : EventHandlerDecoratorBase<T> where T : IEvent
{
    public NullEventValidationEventHandler(IHandleEvent<T> eventHandler,
        IGetDecoratedInstance decoratorService) : base(eventHandler, decoratorService)
    {

    }

    public override Task HandleAsync(T? @event)
    {
        ArgumentNullException.ThrowIfNull(@event);
        return Decorated.HandleAsync(@event);
    }
}