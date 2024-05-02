using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Dispatching.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.Events;

internal class EventServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider =>
        {
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();

            IRaiseDomainEvents eventRaiser = new DomainEventRaiser(eventDispatcher, logger);

            return eventRaiser;
        });
    }
}
