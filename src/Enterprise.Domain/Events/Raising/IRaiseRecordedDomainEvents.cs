using Enterprise.Events.Raising.Callbacks.Abstractions;

namespace Enterprise.Domain.Events.Raising;

/// <summary>
/// Raises domain events recorded by entities and optionally executes callbacks using an external callback service.
/// </summary>
public interface IRaiseRecordedDomainEvents
{
    /// <summary>
    /// Raise domain events recorded by each entity and execute any registered callbacks associated with each event.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="callbackService"></param>
    /// <returns></returns>
    Task RaiseAsync(IEnumerable<IGetDomainEvents> entities, IRaiseEventCallbacks? callbackService = null);

    /// <summary>
    /// Raise domain events recorded by the entity and execute any registered callbacks associated with the event.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="callbackService"></param>
    /// <returns></returns>
    Task RaiseAsync(IGetDomainEvents entity, IRaiseEventCallbacks? callbackService = null);
}
