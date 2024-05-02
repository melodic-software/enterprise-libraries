using Enterprise.Events.Model;
using Enterprise.Events.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Raising.Callbacks.Model;
using Enterprise.Events.Raising.Callbacks.Model.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Callbacks;

public class EventCallbackRegistrar : IEventCallbackRegistrar
{
    private readonly ILogger<EventCallbackRegistrar> _logger;
    private readonly Dictionary<Type, IEnumerable<IEventCallback>> _callbackDictionary;

    public EventCallbackRegistrar(ILogger<EventCallbackRegistrar> logger)
    {
        _logger = logger;
        _callbackDictionary = new Dictionary<Type, IEnumerable<IEventCallback>>();
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        IEventCallback<TEvent> eventCallback = new EventCallback<TEvent>(action);

        Type eventType = typeof(TEvent);

        bool noCallbacksRegisteredForEvent = !_callbackDictionary.ContainsKey(eventType);

        if (noCallbacksRegisteredForEvent)
        {
            IEnumerable<IEventCallback<TEvent>> eventCallbacks = [eventCallback];
            _callbackDictionary.Add(eventType, eventCallbacks);
        }
        else
        {
            if (_callbackDictionary[eventType] is not IEnumerable<IEventCallback<TEvent>> existingCallbacks)
                throw new Exception("Existing callback list is not valid.");

            List<IEventCallback<TEvent>> existingCallbackList = existingCallbacks.ToList();

            // Check for duplicates before adding
            if (!existingCallbackList.Any(x => x.Equals(eventCallback)))
                existingCallbackList.Add(eventCallback);
            else
                _logger.LogWarning("Attempted to register a duplicate callback.");
        }

        _logger.LogDebug("Callback successfully registered.");

        int totalCallbacksForEvent = _callbackDictionary[eventType].Count();
        int totalCallbacks = _callbackDictionary.Sum(x => x.Value.Count());

        _logger.LogDebug(
            "Total callbacks for event: {TotalCallbacksForEvent}. " +
            "Total callbacks registered: {TotalCallbacks}.",
            totalCallbacksForEvent, totalCallbacks
        );
    }

    /// <inheritdoc />
    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks()
    {
        return _callbackDictionary;
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _callbackDictionary.Clear();
    }
}
