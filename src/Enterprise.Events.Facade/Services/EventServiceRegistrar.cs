using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Facade.Services;

internal class EventServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<IEventServiceFacade>(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IRaiseDomainEvents domainEventRaiser = provider.GetRequiredService<IRaiseDomainEvents>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
            ILogger<EventServiceFacade> logger = provider.GetRequiredService<ILogger<EventServiceFacade>>();

            return new EventServiceFacade(eventRaiser, domainEventRaiser, eventCallbackService, logger);
        });
    }
}
