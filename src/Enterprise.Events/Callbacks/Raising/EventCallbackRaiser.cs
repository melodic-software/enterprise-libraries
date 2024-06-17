using Enterprise.Events.Callbacks.Model.NonGeneric;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Callbacks.Registration.Abstract;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Callbacks.Raising;

public class EventCallbackRaiser : IRaiseEventCallbacks
{
    private readonly IGetRegisteredCallbacks _eventCallbackRegistrar;
    private readonly ILogger<EventCallbackRaiser> _logger;
    private readonly bool _allowMultipleExecutions;

    public EventCallbackRaiser(IGetRegisteredCallbacks eventCallbackRegistrar, ILogger<EventCallbackRaiser> logger, bool? allowMultipleExecutions = null)
    {
        _eventCallbackRegistrar = eventCallbackRegistrar;
        _logger = logger;
        _allowMultipleExecutions = allowMultipleExecutions ?? false;
    }

    /// <inheritdoc />
    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        foreach (IEvent @event in events)
        {
            RaiseCallbacks(@event);
        }
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

        var callbackList = callbacks.ToList();

        _logger.LogDebug("Attempting to raise {CallbackCount} callback(s).", callbackList.Count);

        foreach (IEventCallback callback in callbackList)
        {
            RaiseCallback(@event, callback);
        }
    }

    private void RaiseCallback<TEvent>(TEvent @event, IEventCallback callback) where TEvent : IEvent
    {
        if (!_allowMultipleExecutions && callback.HasBeenExecuted)
        {
            _logger.LogInformation(
                "Callback has already been executed and callbacks have been configured to only execute once. " +
                "Execution is being skipped to avoid duplication."
            );

            return;
        }

        if (callback.IsFor(@event))
        {
            callback.Execute(@event);
        }
        else
        {
            _logger.LogWarning("Callback has not been executed due to a type mismatch.");
        }
    }
}
