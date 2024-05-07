namespace Enterprise.Domain.Events.Queuing;

public class DomainEventQueueConfigOptions
{
    public const string ConfigSectionKey = "Custom:DomainEventQueuing";

    /// <summary>
    /// When enabled, the domain events will no longer be raised in real time.
    /// Instead, domain events will be added to a queue stored in the scoped HTTP context items collection.
    /// Unless configured otherwise, callbacks will immediately be executed.
    /// The domain events themselves will be raised after the response has been issued.
    /// The idea here to process side effects in the background.
    /// Technically the use case may not be considered complete until these events have been handled.
    /// Mechanisms may need to be in place to notify users of failure.
    /// </summary>
    public bool EnableDomainEventQueuing { get; set; } = false;
}
