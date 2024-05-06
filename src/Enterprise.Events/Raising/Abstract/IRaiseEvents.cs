using Enterprise.Events.Model;

namespace Enterprise.Events.Raising.Abstract;

/// <summary>
/// Raises events.
/// </summary>
public interface IRaiseEvents
{
    /// <summary>
    /// Raises events.
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    Task RaiseAsync(IReadOnlyCollection<IEvent> events);

    /// <summary>
    /// Raise an event.
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    Task RaiseAsync(IEvent @event);
}
