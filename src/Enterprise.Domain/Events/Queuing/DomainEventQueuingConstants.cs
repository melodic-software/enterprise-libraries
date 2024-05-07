namespace Enterprise.Domain.Events.Queuing;

public static class HttpContextDomainEventQueuingConstants
{
    /// <summary>
    /// Key for accessing the domain events queue stored in the items collection of a scoped HTTP request context.
    /// </summary>
    public const string DomainEventQueueKey = "DomainEventsQueueKey";
}
