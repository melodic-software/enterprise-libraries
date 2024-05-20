using Enterprise.DateTimes.Current.Abstract;
using Enterprise.Events.Model;
using Enterprise.Patterns.Outbox.Model;
using Enterprise.Serialization.Json;

namespace Enterprise.Patterns.Outbox.Factory;

public class EventOutboxMessageFactory
{
    private readonly ISerializeJson _jsonSerializer;
    private readonly IDateTimeUtcNowProvider _dateTimeProvider;

    public EventOutboxMessageFactory(ISerializeJson jsonSerializer, IDateTimeUtcNowProvider dateTimeProvider)
    {
        _jsonSerializer = jsonSerializer;
        _dateTimeProvider = dateTimeProvider;
    }

    public IReadOnlyCollection<EventOutboxMessage> CreateFrom(IReadOnlyCollection<IEvent> values)
    {
        if (!values.Any())
        {
            return new List<EventOutboxMessage>();
        }

        // Projecting events into outbox message instances.
        List<EventOutboxMessage> outboxMessages = values
            .Select(value => new EventOutboxMessage(
                Guid.NewGuid(),
                value.GetType().Name,
                _jsonSerializer.Serialize(value),
                _dateTimeProvider.UtcNow))
            .ToList();

        return outboxMessages;
    }
}
