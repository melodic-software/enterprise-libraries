using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Handlers;

public static class EventHandlerTypeValidationService
{
    public static void ValidateType<TEvent>(IEvent @event, IHandleEvent eventHandler) where TEvent : IEvent
    {
        ValidateType(@event, typeof(TEvent), eventHandler);
    }

    public static void ValidateType<TEvent>(IEvent @event, IHandleEvent<TEvent> eventHandler) where TEvent : IEvent
    {
        ValidateType(@event, typeof(TEvent), eventHandler);
    }

    public static void ValidateType(IEvent @event, Type expectedEventType, IHandleEvent eventHandler)
    {
        Type eventType = @event.GetType();

        bool eventCanBeHandled = eventType.IsAssignableFrom(expectedEventType);

        if (eventCanBeHandled)
            return;

        Type eventHandlerType = eventHandler.GetType();

        throw new InvalidOperationException(EventCannotBeHandled(eventType, eventHandlerType));
    }

    private static string EventCannotBeHandled(Type eventType, Type eventHandlerType) =>
        $"An event of type \"{eventType.FullName}\" cannot be handled by \"{eventHandlerType.FullName}\"";
}