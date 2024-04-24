namespace Enterprise.EntityFramework.AspNetCore.EventualConsistency;

public static class DeferredDomainEventQueueConstants
{
    /// <summary>
    /// Key for accessing the domain events queue stored in the HTTP context.
    /// </summary>
    public const string DomainEventQueueKey = "DomainEventsQueueKey";
}