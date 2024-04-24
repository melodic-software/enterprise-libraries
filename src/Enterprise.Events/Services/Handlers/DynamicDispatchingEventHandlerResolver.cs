using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Handlers;

public class DynamicDispatchingEventHandlerResolver : EventHandlerResolverBase
{
    public DynamicDispatchingEventHandlerResolver(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    /// <inheritdoc />
    public override async Task<IEnumerable<IHandleEvent>> ResolveAsync(IEvent @event)
    {
        return await ResolveAsync((dynamic)@event);
    }
}