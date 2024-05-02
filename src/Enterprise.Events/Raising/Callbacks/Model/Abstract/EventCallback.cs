using Enterprise.Events.Model;

namespace Enterprise.Events.Raising.Callbacks.Model.Abstract;

public class EventCallback<T> : IEventCallback<T> where T : IEvent
{
    public Action<T> Action { get; }
    public bool HasBeenExecuted { get; protected set; }

    public EventCallback(Action<T> action)
    {
        Action = action;
    }

    /// <inheritdoc />
    public bool IsFor(IEvent @event)
    {
        var genericType = typeof(T);
        var eventType = @event.GetType();
        var typesMatch = genericType == eventType;
        return typesMatch;
    }

    /// <inheritdoc />
    public void Execute(IEvent @event)
    {
        if (!IsFor(@event))
            return;

        Execute((T)@event);
    }

    /// <inheritdoc />
    public void Execute(T @event)
    {
        Action.Invoke(@event);
        HasBeenExecuted = true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is EventCallback<T> otherCallback)
            return Equals(Action, otherCallback.Action);

        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = 17;
        hash = hash * 31 + (Action?.Method.GetHashCode() ?? 0);
        hash = hash * 31 + (Action?.Target?.GetHashCode() ?? 0);
        return hash;
    }

    private static bool Equals(Delegate? a, Delegate? b)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;

        var areEqual = a.Method.Equals(b.Method) && Equals(a.Target, b.Target);

        return areEqual;
    }
}
