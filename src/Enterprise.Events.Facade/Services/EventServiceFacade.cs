using Enterprise.Domain.Events;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Callbacks.Model;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Facade.Services;

/// <summary>
/// <see cref="EventServiceFacade"/> centralizes event-related functionalities for ease of use.
/// It wraps the event raising and callback handling, delegating to specific services.
/// This approach simplifies event management in the application by providing a cleaner
/// and more streamlined interface, reducing direct dependencies on multiple event services.
/// </summary>
public class EventServiceFacade : IEventServiceFacade
{
    private readonly HashSet<Guid> _processedEventIds = [];

    private readonly IRaiseEvents _eventRaiser;
    private readonly IRaiseDomainEvents _domainEventRaiser;
    private readonly IEventCallbackService _eventCallbackService;
    private readonly ILogger<EventServiceFacade> _logger;

    public EventServiceFacade(IRaiseEvents eventRaiser,
        IRaiseDomainEvents domainEventRaiser,
        IEventCallbackService eventCallbackService,
        ILogger<EventServiceFacade> logger)
    {
        _eventRaiser = eventRaiser;
        _domainEventRaiser = domainEventRaiser;
        _eventCallbackService = eventCallbackService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IEvent @event)
    {
        if (_processedEventIds.Contains(@event.Id))
        {
            _logger.LogWarning("Event with ID \"{EventId}\" has already been processed.", @event.Id);
            return;
        }

        await _eventRaiser.RaiseAsync(@event);

        _processedEventIds.Add(@event.Id);
    }

    /// <inheritdoc />
    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        await _eventRaiser.RaiseAsync(events);
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

    /// <inheritdoc />
    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks()
    {
        return _eventCallbackService.GetRegisteredCallbacks();
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        _eventCallbackService.RegisterEventCallback(eventCallback);
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
