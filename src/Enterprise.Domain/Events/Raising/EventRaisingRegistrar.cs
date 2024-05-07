using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Queuing;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Options.Core.Singleton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.Events.Raising;

internal class EventRaisingRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        DomainEventQueueConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<DomainEventQueueConfigOptions>(configuration, DomainEventQueueConfigOptions.ConfigSectionKey);

        if (configOptions.EnableDomainEventQueuing)
            return;

        services.AddSingleton(provider =>
        {
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();

            IRaiseDomainEvents eventRaiser = new DomainEventRaiser(eventDispatcher, logger);

            return eventRaiser;
        });
    }
}
