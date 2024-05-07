using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Queuing;
using Enterprise.Domain.Events.Queuing.Decoration;
using Enterprise.Domain.Events.Queuing.Options;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Options.Core.Singleton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.Events.Raising;

internal class EventRaisingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        DomainEventQueuingConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<DomainEventQueuingConfigOptions>(configuration, DomainEventQueuingConfigOptions.ConfigSectionKey);

        if (configOptions.EnableDomainEventQueuing)
        {
            // This makes it so all domain events are queued instead of being immediately raised.
            services.BeginRegistration<IRaiseDomainEvents>()
                .AddScoped(provider =>
                {
                    IEnqueueDomainEvents domainEventQueueService = provider.GetRequiredService<IEnqueueDomainEvents>();
                    return new QueueingDomainEventRaiser(domainEventQueueService);
                })
                .WithDecorator((provider, eventRaiser) =>
                {
                    IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                    IRaiseEventCallbacks callbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
                    return new DomainEventCallbackRaisingDecorator(eventRaiser, decoratorService, callbackRaiser);
                });
        }
        else
        {
            // Primary registration for normal event raising with callbacks.
            services.TryAddScoped<IRaiseDomainEvents>(provider =>
            {
                // Callbacks are raised immediately after events are dispatched (likely via decorator).
                IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
                ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();
                return new DomainEventRaiser(eventDispatcher, logger);
            });
        }

        // Secondary registration for queued event raising without immediate callbacks.
        services.TryAddSingleton<IRaiseQueuedDomainEvents>(provider =>
        {
            // We get a separate (more specific) dispatching interface that implements IDispatchEvents.
            // This implementation is one that does not utilize event callback raising.
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchQueuedEvents>();
            ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();
            return new DomainEventRaiser(eventDispatcher, logger);
        });
    }
}
