using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Facade.Services;
using Enterprise.Events.Services.Raising.Abstract;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Facade.Dependencies;

internal class EventFacadeRegistrar : IRegisterServices
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