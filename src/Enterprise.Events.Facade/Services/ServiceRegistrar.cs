using Enterprise.DI.Core.Registration;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.Events.Facade.Services;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<IEventRaisingFacade>(provider =>
        {
            IRaiseEvents eventRaiser = provider.GetRequiredService<IRaiseEvents>();
            IRaiseDomainEvents domainEventRaiser = provider.GetRequiredService<IRaiseDomainEvents>();

            return new EventRaisingFacade(eventRaiser, domainEventRaiser);
        });

        services.TryAddScoped<IEventServiceFacade>(provider =>
        {
            IEventRaisingFacade eventRaisingFacade = provider.GetRequiredService<IEventRaisingFacade>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();

            return new EventServiceFacade(eventRaisingFacade, eventCallbackService);
        });
    }
}
