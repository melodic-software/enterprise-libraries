using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Decorators;

public class DuplicatePreventingEventRaiser : DecoratorBase<IRaiseEvents>, IRaiseEvents
{
    private readonly IRecordRaisedEvents _raisedEventRecorder;
    private readonly ILogger<DuplicatePreventingEventRaiser> _logger;

    public DuplicatePreventingEventRaiser(IRaiseEvents decorated,
        IGetDecoratedInstance decoratorService,
        IRecordRaisedEvents raisedEventRecorder,
        ILogger<DuplicatePreventingEventRaiser> logger) : base(decorated, decoratorService)
    {
        _raisedEventRecorder = raisedEventRecorder;
        _logger = logger;
    }

    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        await RaiseAsync(events, e => Decorated.RaiseAsync(events));
    }

    public async Task RaiseAsync(IEvent @event)
    {
        await RaiseAsync(@event, e => Decorated.RaiseAsync(@event));
    }

    private async Task RaiseAsync<T>(IReadOnlyCollection<T> events, Func<IEnumerable<T>, Task> raiseEventsAsync)
        where T : IEvent
    {
        var dedupedEvents = events
            .GroupBy(x => x.Id).Select(x => x.First())
            .ToList();

        int eventCount = events.Count;
        int dedupedEventCount = dedupedEvents.Count;

        if (dedupedEvents.Count != eventCount)
        {
            _logger.LogInformation(
                "Event collection has been deduped. " +
                "Event count: {EventCount}, " +
                "Deduped event count: {DedupedEventCount}",
                eventCount, dedupedEventCount
            );
        }

        var alreadyRaised = dedupedEvents
            .Where(e => _raisedEventRecorder.EventHasBeenRaised(e))
            .ToList();

        var eventsToRaise = dedupedEvents
            .Where(e => !alreadyRaised.Contains(e))
            .ToList();

        alreadyRaised.ForEach(e => LogAlreadyRaised(e));

        await raiseEventsAsync(eventsToRaise);

        eventsToRaise.ForEach(e => _raisedEventRecorder.Record(e));
    }

    private async Task RaiseAsync<T>(T @event, Func<T, Task> raiseEventAsync) where T : IEvent
    {
        if (_raisedEventRecorder.EventHasBeenRaised(@event))
        {
            LogAlreadyRaised(@event);
            return;
        }

        await raiseEventAsync(@event);

        _raisedEventRecorder.Record(@event);
    }

    private void LogAlreadyRaised(IEvent @event)
    {
        _logger.LogWarning("Event with ID \"{EventId}\" has already been raised.", @event.Id);
    }
}
