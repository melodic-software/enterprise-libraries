using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Decoration.Abstract;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Handlers.Decoration;

public class LoggingEventHandler<T> : EventHandlerDecoratorBase<T> where T : IEvent
{
    private readonly ILogger<LoggingEventHandler<T>> _logger;

    public LoggingEventHandler(IHandleEvent<T> eventHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingEventHandler<T>> logger)
        : base(eventHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(T @event, CancellationToken cancellationToken = default)
    {
        using (_logger.BeginScope("Event Handler: {@EventHandler}, Event: {@Event}", Innermost, @event))
        {
            _logger.LogDebug("Handling event.");
            await Decorated.HandleAsync(@event, cancellationToken);
            _logger.LogDebug("Event was handled successfully.");
        }
    }
}
