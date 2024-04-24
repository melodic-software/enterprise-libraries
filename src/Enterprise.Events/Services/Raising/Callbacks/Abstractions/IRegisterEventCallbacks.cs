using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Raising.Callbacks.Abstractions;

public interface IRegisterEventCallbacks
{
    /// <summary>
    /// Register a delegate to react to events that have been raised during use case execution.
    /// These are typically domain events.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    /// <param name="action">The action to invoke when the event is raised.</param>
    void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent;
}