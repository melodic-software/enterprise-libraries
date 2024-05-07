﻿using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Queuing;
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
        DomainEventQueueConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<DomainEventQueueConfigOptions>(configuration, DomainEventQueueConfigOptions.ConfigSectionKey);

        if (configOptions.EnableDomainEventQueuing)
            return;

        services.TryAddScoped(provider =>
        {
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();

            IRaiseDomainEvents eventRaiser = new DomainEventRaiser(eventDispatcher, logger);

            return eventRaiser;
        });
    }
}
