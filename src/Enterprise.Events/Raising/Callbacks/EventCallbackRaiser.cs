using Enterprise.Events.Model;
using Enterprise.Events.Raising.Callbacks.Abstractions;
using Enterprise.Events.Raising.Callbacks.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Callbacks;

public class EventCallbackRaiser : IRaiseEventCallbacks
{
    private readonly IGetRegisteredCallbacks _eventCallbackRegistrar;
    private readonly ILogger<EventCallbackRaiser> _logger;

    public EventCallbackRaiser(IGetRegisteredCallbacks eventCallbackRegistrar, ILogger<EventCallbackRaiser> logger)
    {
        _eventCallbackRegistrar = eventCallbackRegistrar;
        _logger = logger;
    }

    /// <inheritdoc />
    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        foreach (var @event in events)
            RaiseCallbacks(@event);
    }

    /// <inheritdoc />
    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = @event.GetType();
        var registeredCallbacks = _eventCallbackRegistrar.GetRegisteredCallbacks();

        var callbacksRegistered = registeredCallbacks.ContainsKey(eventType);

        if (!callbacksRegistered)
        {
            _logger.LogInformation("No callbacks have been registered.");
            return;
        }

        var callbacks = registeredCallbacks[eventType];

        var callbackList = callbacks.ToList();

        _logger.LogDebug("Executing {CallbackCount} callback(s).", callbackList.Count);

        foreach (var callback in callbackList)
            RaiseCallback(@event, callback);
    }

    private void RaiseCallback<TEvent>(TEvent @event, IEventCallback callback) where TEvent : IEvent
    {
        callback.Execute(@event);

        if (callback.HasBeenExecuted)
            return;

        var isTypeMismatch = !callback.IsFor(@event);

        var additionalText = isTypeMismatch ? " due to a type mismatch" : string.Empty;

        _logger.LogWarning("Callback has not been executed" + additionalText + ".");
    }
}