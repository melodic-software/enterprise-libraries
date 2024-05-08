using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.Events.Raising;

public class DomainEventRaiser : EventRaiser, IRaiseDomainEvents
{
    public DomainEventRaiser(IDispatchEvents eventDispatcher, ILogger<EventRaiser> logger)
        : base(eventDispatcher, logger)
    {
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEnumerable<IGetDomainEvents> entities)
    {
        foreach (IGetDomainEvents entity in entities)
            await RaiseAsync(entity);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IGetDomainEvents entity)
    {
        IReadOnlyList<IDomainEvent> events = entity.GetDomainEvents();
        await RaiseAsync(events);
        (entity as IClearDomainEvents)?.ClearDomainEvents(); // TODO: Should we be doing this here? It seems like an obscured side effect.
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        await RaiseAsync((IReadOnlyCollection<IEvent>)domainEvents);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IDomainEvent domainEvent)
    {
        await RaiseAsync((IEvent)domainEvent);
    }
}
