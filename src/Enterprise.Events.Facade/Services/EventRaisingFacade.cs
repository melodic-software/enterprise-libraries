using Enterprise.Domain.Events;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;

namespace Enterprise.Events.Facade.Services;

public  class EventRaisingFacade : IEventRaisingFacade
{
    private readonly IRaiseEvents _eventRaiser;
    private readonly IRaiseDomainEvents _domainEventRaiser;

    public EventRaisingFacade(IRaiseEvents eventRaiser,
        IRaiseDomainEvents domainEventRaiser)
    {
        _eventRaiser = eventRaiser;
        _domainEventRaiser = domainEventRaiser;
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        await _eventRaiser.RaiseAsync(events);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEvent @event)
    {
        await _eventRaiser.RaiseAsync(@event);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEnumerable<IGetDomainEvents> entities)
    {
        await _domainEventRaiser.RaiseAsync(entities);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IGetDomainEvents entity)
    {
        await _domainEventRaiser.RaiseAsync(entity);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        await _domainEventRaiser.RaiseAsync(domainEvents);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IDomainEvent domainEvent)
    {
        await _domainEventRaiser.RaiseAsync(domainEvent);
    }
}
