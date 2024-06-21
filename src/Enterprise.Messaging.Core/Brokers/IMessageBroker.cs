using Enterprise.Messaging.Core.Model;

namespace Enterprise.Messaging.Core.Brokers;

public interface IMessageBroker
{
    Task PublishAsync(IMessage message, CancellationToken cancellationToken = default);
    Task PublishAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = default);
}
