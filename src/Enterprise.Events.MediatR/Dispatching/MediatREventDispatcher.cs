using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Dispatching;
using Enterprise.Events.Handlers.Abstract.NonGeneric;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using static Enterprise.Reflection.Types.GenericInterfaceTypeService;

namespace Enterprise.Events.MediatR.Dispatching;

public sealed class MediatREventDispatcher : EventDispatcher
{
    private readonly IPublisher _publisher;
    private bool _handlersFiltered;

    public MediatREventDispatcher(
        IPublisher publisher,
        IGetDecoratedInstance decoratorService,
        IResolveEventHandlers eventHandlerResolver,
        ILogger<MediatREventDispatcher> logger
    ) : base(decoratorService, eventHandlerResolver, logger)
    {
        _publisher = publisher;
    }

    protected override IEnumerable<IHandleEvent> FilterHandlers(ICollection<IHandleEvent> eventHandlers, IEvent @event)
    {
        if (eventHandlers.Count <= 0)
        {
            Logger.LogDebug("No handlers to filter.");
            return eventHandlers;
        }

        int initialCount = eventHandlers.Count;

        // We need to check the implemented interfaces on each handler since we could be utilizing base event handlers AND MediatR notification (event) handlers.
        // We have to make sure we don't call event handlers more than once when we use the base event raising mechanism AND the MediatR publisher.

        var filteredHandlers = eventHandlers
            .Where(IsNotMediatRHandler())
            .ToList();

        int filteredCount = filteredHandlers.Count;

        if (initialCount == filteredCount)
        {
            return filteredHandlers;
        }

        int totalFiltered = Math.Abs(initialCount - filteredCount);
        Logger.LogDebug("Filtered out {FilteredCount} handler(s).", totalFiltered);

        return filteredHandlers;
    }

    protected override Task OnNoHandlersRegistered(IEvent @event)
    {
        if (_handlersFiltered)
        {
            Logger.LogInformation(
                "All registered handlers have implemented both MediatR and base event handling interfaces. " +
                "The MediatR publisher will be used exclusively."
            );
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override async Task OnDispatchCompleteAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        // At this point we've safely filtered out handlers that would be executed twice with the use of the MediatR publisher.
        await PublishAsync(@event);
    }

    private async Task PublishAsync(IEvent @event)
    {
        Logger.LogDebug("Executing MediatR publisher.");
        await _publisher.Publish(@event);
        Logger.LogDebug("Event publication completed.");
    }

    private Func<IHandleEvent, bool> IsNotMediatRHandler()
    {
        Type mediatRInterface = typeof(INotificationHandler<>);

        return x =>
        {
            // IF the event handler has been decorated, this will return the inner handler.
            IHandleEvent eventHandler = DecoratorService.GetInnermost(x);

            Type eventHandlerType = eventHandler.GetType();

            bool implementsMediatRType = ImplementsGenericInterface(eventHandlerType, mediatRInterface);

            if (!implementsMediatRType)
            {
                return true;
            }

            Logger.LogDebug(
                "Handler type \"{EventHandlerType}\" implements {MediatRInterface} and is being filtered out to avoid duplicate handling. " +
                "These handlers must be wired up separately in the DI container via the MediatR service registrations. " +
                "If registered, these handlers will be triggered when the event is sent via an {PublisherInterfaceName} instance.",
                eventHandlerType.Name, mediatRInterface.Name, nameof(IPublisher)
            );

            if (!_handlersFiltered)
            {
                _handlersFiltered = true;
            }

            return false;
        };
    }
}
