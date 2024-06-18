using Enterprise.Domain.Events;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Callbacks.Model.NonGeneric;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Facade.Services;

/// <summary>
/// <see cref="EventServiceFacade"/> centralizes event-related functionalities for ease of use.
/// It wraps the event raising and callback handling, delegating to specific services.
/// This approach simplifies event management in the application by providing a cleaner
/// and more streamlined interface, reducing direct dependencies on multiple event services.
/// </summary>
public class EventServiceFacade : IEventServiceFacade
{
    private readonly IEventRaisingFacade _eventRaisingFacade;
    private readonly IEventCallbackService _eventCallbackService;

    public EventServiceFacade(IEventRaisingFacade eventRaisingFacade,
        IEventCallbackService eventCallbackService)
    {
        _eventRaisingFacade = eventRaisingFacade;
        _eventCallbackService = eventCallbackService;
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        await _eventRaisingFacade.RaiseAsync(events);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEvent @event)
    {
        await _eventRaisingFacade.RaiseAsync(@event);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEnumerable<IGetDomainEvents> entities)
    {
        await _eventRaisingFacade.RaiseAsync(entities);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IGetDomainEvents entity)
    {
        await _eventRaisingFacade.RaiseAsync(entity);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        await _eventRaisingFacade.RaiseAsync(domainEvents);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IDomainEvent domainEvent)
    {
        await _eventRaisingFacade.RaiseAsync(domainEvent);
    }

    /// <inheritdoc />
    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks()
    {
        return _eventCallbackService.GetRegisteredCallbacks();
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        _eventCallbackService.RegisterEventCallback(action);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _eventCallbackService.ClearCallbacks();
    }

    /// <inheritdoc />
    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        _eventCallbackService.RaiseCallbacks(events);
    }

    /// <inheritdoc />
    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        _eventCallbackService.RaiseCallbacks(@event);
    }
}
