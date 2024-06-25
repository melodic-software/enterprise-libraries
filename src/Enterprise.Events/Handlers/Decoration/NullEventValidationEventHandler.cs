using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Decoration.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Decoration;

public class NullEventValidationEventHandler<T> : EventHandlerDecoratorBase<T> where T : IEvent
{
    public NullEventValidationEventHandler(IHandleEvent<T> eventHandler,
        IGetDecoratedInstance decoratorService) : base(eventHandler, decoratorService)
    {

    }

    public override Task HandleAsync(T? @event, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);
        return Decorated.HandleAsync(@event, cancellationToken);
    }
}
