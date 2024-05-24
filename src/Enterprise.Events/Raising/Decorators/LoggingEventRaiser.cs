using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Decorators;

public class LoggingEventRaiser : DecoratorBase<IRaiseEvents>, IRaiseEvents
{
    private readonly ILogger<LoggingEventRaiser> _logger;

    public LoggingEventRaiser(IRaiseEvents decorated,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingEventRaiser> logger) : base(decorated, decoratorService)
    {
        _logger = logger;
    }

    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        _logger.LogDebug("Raising {EventCount} events.", events.Count);
        await Decorated.RaiseAsync(events);
        _logger.LogDebug("{EventCount} event(s) raised.", events.Count);
    }

    public async Task RaiseAsync(IEvent @event)
    {
        using (_logger.BeginScope("Event: {EventType}", @event.GetType().Name))
        {
            await Decorated.RaiseAsync(@event);
        }
    }
}
