using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Handlers;
using Enterprise.Events.Services.Handlers.Decoration;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Decorators.EventHandlers;

public class ErrorHandlingEventHandler<T> : EventHandlerDecoratorBase<T> where T : IEvent
{
    private readonly ILogger<ErrorHandlingEventHandler<T>> _logger;

    public ErrorHandlingEventHandler(IHandleEvent<T> eventHandler, IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingEventHandler<T>> logger) : base(eventHandler, decoratorService)
    {
        _logger = logger;
    }

    public override Task HandleAsync(T @event)
    {
        try
        {
            return Decorated.HandleAsync(@event);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the event.");
            throw;
        }
    }
}