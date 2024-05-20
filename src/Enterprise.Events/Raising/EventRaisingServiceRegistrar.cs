using Enterprise.DI.Core.Registration;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Raising.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising;

internal sealed class EventRaisingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<IRaiseEvents>(provider =>
        {
            IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
            ILogger<EventRaiser> logger = provider.GetRequiredService<ILogger<EventRaiser>>();
            return new EventRaiser(eventDispatcher, logger);
        });
    }
}
