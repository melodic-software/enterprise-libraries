using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Resolution;

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
