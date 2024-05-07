using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Facade.Services;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Facade.Dependencies;

internal class EventFacadeServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IRaiseDomainEvents domainEventRaiser = provider.GetRequiredService<IRaiseDomainEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
            ILogger<EventServiceFacade> logger = provider.GetRequiredService<ILogger<EventServiceFacade>>();

            IEventServiceFacade eventService = new EventServiceFacade(eventRaiser, domainEventRaiser, eventCallbackService, logger);

            return eventService;
        });
    }
}
