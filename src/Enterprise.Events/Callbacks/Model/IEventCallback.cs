using Enterprise.Events.Callbacks.Model.NonGeneric;
using Enterprise.Events.Model;

namespace Enterprise.Events.Callbacks.Model;

public interface IEventCallback<in T> : IEventCallback where T : IEvent
{
    /// <summary>
    /// Execute the callback.
    /// </summary>
    /// <param name="event"></param>
    public void Execute(T @event);
}
