using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Dispatching.Abstract;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.Events.Raising;

public class DomainEventRaiser : EventRaiser, IRaiseDomainEvents
{
    public DomainEventRaiser(IDispatchEvents eventDispatcher, ILogger<EventRaiser> logger)
        : base(eventDispatcher, logger)
    {
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEnumerable<IGetDomainEvents> entities, IRaiseEventCallbacks? eventCallbackService)
    {
        foreach (IGetDomainEvents entity in entities)
            await RaiseAsync(entity, eventCallbackService);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IGetDomainEvents entity, IRaiseEventCallbacks? eventCallbackService)
    {
        IReadOnlyList<IDomainEvent> events = entity.GetDomainEvents();
        await RaiseAsync(events, eventCallbackService);
        (entity as IClearDomainEvents)?.ClearDomainEvents(); // TODO: Should we be doing this here? It seems like an obscured side effect.
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents, IRaiseEventCallbacks? eventCallbackService)
    {
        await RaiseAsync((IReadOnlyCollection<IEvent>)domainEvents, eventCallbackService);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IDomainEvent domainEvent, IRaiseEventCallbacks? eventCallbackService)
    {
        await RaiseAsync((IEvent)domainEvent, eventCallbackService);
    }
}