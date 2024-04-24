using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.Events.Services.Raising.Callbacks.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Services.Raising.Callbacks;

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
        foreach (IEvent @event in events)
            RaiseCallbacks(@event);
    }

    /// <inheritdoc />
    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        Type eventType = @event.GetType();
        Dictionary<Type, IEnumerable<IEventCallback>> registeredCallbacks = _eventCallbackRegistrar.GetRegisteredCallbacks();

        bool callbacksRegistered = registeredCallbacks.ContainsKey(eventType);

        if (!callbacksRegistered)
        {
            _logger.LogInformation("No callbacks have been registered.");
            return;
        }

        IEnumerable<IEventCallback> callbacks = registeredCallbacks[eventType];

        List<IEventCallback> callbackList = callbacks.ToList();

        _logger.LogDebug("Executing {CallbackCount} callback(s).", callbackList.Count);

        foreach (IEventCallback callback in callbackList)
            RaiseCallback(@event, callback);
    }

    private void RaiseCallback<TEvent>(TEvent @event, IEventCallback callback) where TEvent : IEvent
    {
        callback.Execute(@event);

        if (callback.HasBeenExecuted)
            return;

        bool isTypeMismatch = !callback.IsFor(@event);

        string additionalText = isTypeMismatch ? " due to a type mismatch" : string.Empty;

        _logger.LogWarning("Callback has not been executed" + additionalText + ".");
    }
}