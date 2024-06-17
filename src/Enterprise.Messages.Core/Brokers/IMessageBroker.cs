using Enterprise.Messages.Core.Model;

namespace Enterprise.Messages.Core.Brokers;

public interface IMessageBroker
{
    Task PublishAsync(IMessage message, CancellationToken cancellationToken = default);
    Task PublishAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = default);
}
