using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;
using Enterprise.Events.Callbacks.Raising.Abstract;

namespace Enterprise.Events.Callbacks.Raising.Decoration;

public class LoggingEventCallbackRaiser : IRaiseEventCallbacks
{
    private readonly IRaiseEventCallbacks _decoratedCallbackRaiser;
    private readonly ILogger<LoggingEventCallbackRaiser> _logger;

    public LoggingEventCallbackRaiser(IRaiseEventCallbacks decoratedCallbackRaiser,
        ILogger<LoggingEventCallbackRaiser> logger)
    {
        _decoratedCallbackRaiser = decoratedCallbackRaiser;
        _logger = logger;
    }

    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        // We want to use the logging scope for each event.
        // We iterate here and use the other method instead of forwarding on to the delegated instance.

        List<IEvent> eventList = events.ToList();

        _logger.LogDebug("Raising callbacks for {EventCount} events.", eventList.Count);

        foreach (IEvent @event in eventList)
            RaiseCallbacks(@event);

        _logger.LogDebug("Callback raising completed.");
    }

    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        using (_logger.BeginScope("Event: {EventType}", @event.GetType().Name))
        {
            _logger.LogDebug("Raising event callbacks.");
            _decoratedCallbackRaiser.RaiseCallbacks(@event);
            _logger.LogDebug("Event callbacks raised.");
        }
    }
}
