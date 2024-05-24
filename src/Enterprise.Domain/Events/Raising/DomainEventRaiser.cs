using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Raising.Abstract;

namespace Enterprise.Domain.Events.Raising;

public class DomainEventRaiser : IRaiseDomainEvents
{
    private readonly IRaiseEvents _eventRaiser;

    public DomainEventRaiser(IRaiseEvents eventRaiser)
    {
        _eventRaiser = eventRaiser;
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEnumerable<IGetDomainEvents> entities)
    {
        foreach (IGetDomainEvents entity in entities)
        {
            await RaiseAsync(entity);
        }
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
        await _eventRaiser.RaiseAsync(domainEvents);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IDomainEvent domainEvent)
    {
        await _eventRaiser.RaiseAsync(domainEvent);
    }
}
