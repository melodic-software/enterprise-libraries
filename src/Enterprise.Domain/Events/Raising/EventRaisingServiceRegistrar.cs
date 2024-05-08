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
        services.TryAddScoped<IRaiseDomainEvents>(provider =>
        {
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<DomainEventRaiser> logger = provider.GetRequiredService<ILogger<DomainEventRaiser>>();
            return new DomainEventRaiser(eventDispatcher, logger);
        });
    }
}
