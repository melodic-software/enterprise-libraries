namespace Enterprise.Domain.Events.Raising.Abstract;

/// <summary>
/// Raises domain events recorded by entities and optionally executes callbacks using an external callback service.
/// </summary>
public interface IRaiseRecordedDomainEvents
{
    /// <summary>
    /// Raise domain events recorded by each entity and execute any registered callbacks associated with each event.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task RaiseAsync(IEnumerable<IGetDomainEvents> entities);

    /// <summary>
    /// Raise domain events recorded by the entity and execute any registered callbacks associated with the event.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task RaiseAsync(IGetDomainEvents entity);
}
