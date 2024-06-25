using Enterprise.Events.Handlers.Abstract;

namespace Example.Api.Events;

public class MyEventHandler : EventHandlerBase<MyEvent>
{
    public override Task HandleAsync(MyEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
