using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Events.Handlers.Resolution.Abstract;

public abstract class EventHandlerResolverBase : IResolveEventHandlers
{
    private readonly IServiceProvider _serviceProvider;

    protected EventHandlerResolverBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public abstract Task<IEnumerable<IHandleEvent>> ResolveAsync(IEvent @event);

    /// <inheritdoc />
    public Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent
    {
        var handlers = _serviceProvider.GetServices<IHandleEvent<T>>();

        return Task.FromResult(handlers);
    }
}
