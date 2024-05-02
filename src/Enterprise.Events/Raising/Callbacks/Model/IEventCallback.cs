using Enterprise.Events.Model;

namespace Enterprise.Events.Raising.Callbacks.Model;

public interface IEventCallback
{
    /// <summary>
    /// Has the callback already been executed?
    /// </summary>
    public bool HasBeenExecuted { get; }

    /// <summary>
    /// Is the callback action for the specified event type?
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public bool IsFor(IEvent @event);

    /// <summary>
    /// Execute the callback.
    /// </summary>
    /// <param name="event"></param>
    public void Execute(IEvent @event);
}

public interface IEventCallback<in T> : IEventCallback where T : IEvent
{
    /// <summary>
    /// Execute the callback.
    /// </summary>
    /// <param name="event"></param>
    public void Execute(T @event);
}