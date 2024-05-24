using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Decorators.Abstract;

public class DuplicatePreventionDecoratorBase<TDecorated> : DecoratorBase<TDecorated> where TDecorated : class
{
    private readonly IRecordRaisedEvents _raisedEventRecorder;
    private readonly ILogger<DuplicatePreventionDecoratorBase<TDecorated>> _logger;

    public DuplicatePreventionDecoratorBase(TDecorated decorated,
        IGetDecoratedInstance decoratorService,
        IRecordRaisedEvents raisedEventRecorder,
        ILogger<DuplicatePreventionDecoratorBase<TDecorated>> logger) : base(decorated, decoratorService)
    {
        _raisedEventRecorder = raisedEventRecorder;
        _logger = logger;
    }

    protected async Task RaiseAsync<T>(IReadOnlyCollection<T> events, Func<IEnumerable<T>, Task> raiseEvents) where T : IEvent
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

        await raiseEvents.Invoke(eventsToRaise);

        eventsToRaise.ForEach(e => _raisedEventRecorder.Record(e));
    }

    protected async Task RaiseAsync<T>(T @event, Func<T, Task> raiseEvent) where T : IEvent
    {
        if (_raisedEventRecorder.EventHasBeenRaised(@event))
        {
            LogAlreadyRaised(@event);
            return;
        }

        await raiseEvent.Invoke(@event);

        _raisedEventRecorder.Record(@event);
    }

    protected void LogAlreadyRaised(IEvent @event)
    {
        _logger.LogWarning("Event with ID \"{EventId}\" has already been raised.", @event.Id);
    }
}
