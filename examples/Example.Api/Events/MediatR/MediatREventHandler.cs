using Enterprise.Events.MediatR.Handlers;

namespace Example.Api.Events.MediatR;

public class MediatREventHandler : MediatREventHandlerBase<MyEvent>
{
    public override Task HandleAsync(MyEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
