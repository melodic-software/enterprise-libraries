using Enterprise.Events.Model;

namespace Enterprise.Events.Raising.Callbacks.Abstractions;

public interface IRaiseEventCallbacks
{
    /// <summary>
    /// Raise the callbacks for each of the events in the collection.
    /// </summary>
    /// <param name="events"></param>
    void RaiseCallbacks(IEnumerable<IEvent> events);

    /// <summary>
    /// Raise callbacks for a single event.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent;
}