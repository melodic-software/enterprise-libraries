using Enterprise.Events.Model;
using Enterprise.Events.Raising.Callbacks.Abstractions;

namespace Enterprise.Events.Raising.Abstract;

/// <summary>
/// Raises events and optionally executes callbacks using an external callback service.
/// </summary>
public interface IRaiseEvents
{
    /// <summary>
    /// Raises events and executes any registered callbacks associated with each event.
    /// </summary>
    /// <param name="events"></param>
    /// <param name="callbackService"></param>
    /// <returns></returns>
    Task RaiseAsync(IReadOnlyCollection<IEvent> events, IRaiseEventCallbacks? callbackService = null);

    /// <summary>
    /// Raise an event and execute any registered callbacks associated with the event.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="callbackService"></param>
    /// <returns></returns>
    Task RaiseAsync(IEvent @event, IRaiseEventCallbacks? callbackService = null);
}