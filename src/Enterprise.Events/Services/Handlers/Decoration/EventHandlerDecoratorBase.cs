using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using static Enterprise.Events.Services.Handlers.EventHandlerTypeValidationService;

namespace Enterprise.Events.Services.Handlers.Decoration;

public abstract class EventHandlerDecoratorBase<T> : DecoratorBase<IHandleEvent<T>>, IHandleEvent<T> where T : IEvent
{
    protected EventHandlerDecoratorBase(IHandleEvent<T> eventHandler, IGetDecoratedInstance decoratorService) : base(
        eventHandler, decoratorService)
    {
    }

    /// <inheritdoc />
    public Task HandleAsync(IEvent @event)
    {
        ValidateType(@event, this);
        T typedEvent = (T)@event;
        return HandleAsync(typedEvent);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(T @event);
}