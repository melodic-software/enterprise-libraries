using System.Collections.Concurrent;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;

namespace Enterprise.Events.Raising;

public class RaisedEventRecorder : IRecordRaisedEvents
{
    private readonly ConcurrentDictionary<Guid, byte> _raisedEventIds = [];

    public bool EventHasBeenRaised(IEvent @event)
    {
        return _raisedEventIds.ContainsKey(@event.Id);
    }

    public void Record(IEvent @event)
    {
        _raisedEventIds.TryAdd(@event.Id, byte.MinValue);
    }
}
