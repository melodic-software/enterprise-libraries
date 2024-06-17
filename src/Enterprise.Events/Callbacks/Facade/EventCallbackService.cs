using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Callbacks.Model.NonGeneric;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Callbacks.Registration.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Callbacks.Facade;

/// <summary>
/// This is a simple facade service that aggregates the registration and raising of event callbacks.
/// </summary>
public class EventCallbackService : IEventCallbackService
{
    private readonly IEventCallbackRegistrar _callbackRegistrar;
    private readonly IRaiseEventCallbacks _callbackRaiser;

    /// <summary>
    /// This is a simple facade service that aggregates the registration and raising of event callbacks.
    /// </summary>
    public EventCallbackService(IEventCallbackRegistrar callbackRegistrar, IRaiseEventCallbacks callbackRaiser)
    {
        _callbackRegistrar = callbackRegistrar;
        _callbackRaiser = callbackRaiser;
    }

    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks()
    {
        return _callbackRegistrar.GetRegisteredCallbacks();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        _callbackRegistrar.RegisterEventCallback(action);
    }

    public void ClearCallbacks()
    {
        _callbackRegistrar.ClearCallbacks();
    }

    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        _callbackRaiser.RaiseCallbacks(events);
    }

    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        _callbackRaiser.RaiseCallbacks(@event);
    }
}
