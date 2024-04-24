using Enterprise.DateTimes.Current.Abstract;
using Enterprise.Events.Model;
using Enterprise.Patterns.Outbox.Model;
using Enterprise.Serialization.Json;

namespace Enterprise.Patterns.Outbox.Factory;

public class OutboxMessageFactory
{
    private readonly ISerializeJson _jsonSerializer;
    private readonly IDateTimeUtcNowProvider _dateTimeProvider;

    public OutboxMessageFactory(ISerializeJson jsonSerializer, IDateTimeUtcNowProvider dateTimeProvider)
    {
        _jsonSerializer = jsonSerializer;
        _dateTimeProvider = dateTimeProvider;
    }

    public IReadOnlyCollection<OutboxMessage> CreateFrom(IReadOnlyCollection<IEvent> values)
    {
        if (!values.Any())
            return new List<OutboxMessage>();

        // Projecting domain events into outbox message instances.
        List<OutboxMessage> outboxMessages = values
            .Select(value => new OutboxMessage(
                Guid.NewGuid(),
                value.GetType().Name,
                _jsonSerializer.Serialize(value),
                _dateTimeProvider.UtcNow))
            .ToList();

        return outboxMessages;
    }
}