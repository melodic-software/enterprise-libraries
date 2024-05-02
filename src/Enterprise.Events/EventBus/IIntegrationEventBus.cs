namespace Enterprise.Events.EventBus;

public interface IIntegrationEventBus
{
    Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEventBus;
}
