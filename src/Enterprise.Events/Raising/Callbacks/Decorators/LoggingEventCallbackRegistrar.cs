using Enterprise.Events.Model;
using Enterprise.Events.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Raising.Callbacks.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Callbacks.Decorators;

public class LoggingEventCallbackRegistrar : IEventCallbackRegistrar
{
    private readonly IEventCallbackRegistrar _decoratedRegistrar;
    private readonly ILogger<LoggingEventCallbackRegistrar> _logger;

    public LoggingEventCallbackRegistrar(IEventCallbackRegistrar decoratedRegistrar,
        ILogger<LoggingEventCallbackRegistrar> logger)
    {
        _decoratedRegistrar = decoratedRegistrar;
        _logger = logger;
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        using (_logger.BeginScope("Event: {EventType}", typeof(TEvent).Name))
        {
            _logger.LogDebug("Registering event callback.");
            _decoratedRegistrar.RegisterEventCallback(action);
            _logger.LogDebug("Event callback registration complete.");
        }
    }

    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks()
    {
        _logger.LogDebug("Getting registered callbacks.");
        var callbacks = _decoratedRegistrar.GetRegisteredCallbacks();
        _logger.LogDebug("Callbacks retrieved.");
        return callbacks;
    }

    public void ClearCallbacks()
    {
        _logger.LogDebug("Clearing callbacks.");
        _decoratedRegistrar.ClearCallbacks();
        _logger.LogDebug("Callbacks cleared.");
    }
}