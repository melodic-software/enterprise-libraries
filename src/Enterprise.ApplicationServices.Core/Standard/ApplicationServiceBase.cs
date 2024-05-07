using Enterprise.Domain.Events;
using Enterprise.Domain.Events.Model;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Extensions;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Model;
using Enterprise.Patterns.ResultPattern.Errors;

namespace Enterprise.ApplicationServices.Core.Standard;

/// <summary>
/// Application services represent the use cases of the system.
/// They are responsible for orchestrating the components in the domain layer.
/// </summary>
public abstract class ApplicationServiceBase : IApplicationService
{
    private readonly IEventServiceFacade _eventService;

    protected ApplicationServiceBase(IEventServiceFacade eventService)
    {
        _eventService = eventService;
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        _eventService.RegisterEventCallback(eventCallback);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _eventService.ClearCallbacks();
    }

    /// <summary>
    /// Raise domain events recorded by each entity and execute any registered callbacks associated with each event.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    protected async Task RaiseEventsAsync(IEnumerable<IGetDomainEvents> entities)
    {
        await _eventService.RaiseAsync(entities);
    }

    /// <summary>
    /// Raise domain events recorded by the entity and execute any registered callbacks associated with the event.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected async Task RaiseEventsAsync(IGetDomainEvents entity)
    {
        await _eventService.RaiseAsync(entity);
    }

    /// <summary>
    /// Raises events and executes any registered callbacks associated with each event.
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    protected async Task RaiseEventsAsync(IEnumerable<IEvent> events)
    {
        await _eventService.RaiseAsync(events.ToList());
    }

    /// <summary>
    /// Raise the domain events and execute any registered callbacks associated with each event.
    /// </summary>
    /// <param name="domainEvents"></param>
    /// <returns></returns>
    protected async Task RaiseEventsAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        await _eventService.RaiseAsync(domainEvents.ToList());
    }

    /// <summary>
    /// Translates the errors to <see cref="ErrorOccurred"/> instances.
    /// Once translated, the events are raised and any registered callbacks are executed.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    protected async Task RaiseEventsAsync(IEnumerable<IError> errors)
    {
        await RaiseEventsAsync(errors.ToEvents());
    }

    /// <summary>
    /// Raise an event and execute any registered callbacks associated with the event.
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    protected async Task RaiseEventAsync(IEvent @event)
    {
        await _eventService.RaiseAsync(@event);
    }

    /// <summary>
    /// Raise the domain event and execute any registered callbacks associated with the event.
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    protected async Task RaiseEventAsync(IDomainEvent domainEvent)
    {
        await _eventService.RaiseAsync(domainEvent);
    }

    /// <summary>
    /// Translates the errors to a <see cref="ErrorOccurred"/> instance.
    /// Once translated, the events are raised and any registered callbacks are executed.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    protected async Task RaiseEventAsync(IError error)
    {
        await RaiseEventAsync(error.ToEvent());
    }

    /// <summary>
    /// Raise callbacks associated with each event.
    /// </summary>
    /// <param name="events"></param>
    protected void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        _eventService.RaiseCallbacks(events);
    }

    /// <summary>
    /// Raise callbacks associated with the event.
    /// </summary>
    /// <param name="event"></param>
    protected void RaiseCallbacks(IEvent @event)
    {
        _eventService.RaiseCallbacks((dynamic)@event);
    }
}
