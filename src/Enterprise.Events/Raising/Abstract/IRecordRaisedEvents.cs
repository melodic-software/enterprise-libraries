using Enterprise.Events.Model;

namespace Enterprise.Events.Raising.Abstract;

public interface IRecordRaisedEvents
{
    bool EventHasBeenRaised(IEvent @event);
    void Record(IEvent @event);
}
