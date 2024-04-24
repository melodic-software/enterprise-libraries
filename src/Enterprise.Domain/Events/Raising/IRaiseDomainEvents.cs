using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;

namespace Enterprise.Domain.Events.Raising;

/// <summary>
/// Raises domain events and optionally executes callbacks using an external callback service.
/// </summary>
public interface IRaiseDomainEvents : IRaiseRecordedDomainEvents
{
    /// <summary>
    /// Raise domain events and execute any registered callbacks associated with each event.
    /// </summary>
    /// <param name="domainEvents"></param>
    /// <param name="callbackService"></param>
    /// <returns></returns>
    Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents, IRaiseEventCallbacks? callbackService = null);

    /// <summary>
    /// Raise domain events and execute any registered callbacks associated with the event.
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="callbackService"></param>
    /// <returns></returns>
    Task RaiseAsync(IDomainEvent domainEvent, IRaiseEventCallbacks? callbackService = null);
}