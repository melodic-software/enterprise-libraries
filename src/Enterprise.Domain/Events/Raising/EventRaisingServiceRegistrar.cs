using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.Events.Raising;

internal class EventRaisingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Primary registration for normal event raising with callbacks.
        services.TryAddScoped(provider =>
        {
            // Callbacks are raised immediately after events are dispatched (likely via decorator).
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();
            IRaiseDomainEvents eventRaiser = new DomainEventRaiser(eventDispatcher, logger);
            return eventRaiser;
        });

        // Secondary registration for queued event raising without immediate callbacks.
        services.TryAddScoped(provider =>
        {
            // We get a separate (more specific) dispatching interface that implements IDispatchEvents.
            // This implementation is one that does not utilize event callback raising.
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchQueuedEvents>();
            ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();
            IRaiseQueuedDomainEvents eventRaiser = new DomainEventRaiser(eventDispatcher, logger);
            return eventRaiser;
        });
    }
}
