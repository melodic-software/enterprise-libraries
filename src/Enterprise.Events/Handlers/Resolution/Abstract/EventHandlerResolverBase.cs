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
        IServiceProvider serviceProvider = GetServiceProvider();
        IEnumerable<IHandleEvent<T>> handlers = serviceProvider.GetServices<IHandleEvent<T>>();
        return Task.FromResult(handlers);
    }

    private IServiceProvider GetServiceProvider()
    {
        // Check if the current provider is already a scoped service provider.
        if (_serviceProvider.GetService<IServiceScopeFactory>() == null)
        {
            return _serviceProvider;
        }
        
        if (_serviceProvider is IServiceScope)
        {
            return _serviceProvider;
        }

        // Create a new scope if we are in the root scope.
        IServiceScopeFactory scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        IServiceScope scope = scopeFactory.CreateScope();
        return scope.ServiceProvider;
    }
}
