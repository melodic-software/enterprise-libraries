using Enterprise.DI.Core.Registration;
using Enterprise.Domain.Events.Queuing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.AspNetCore.Events.Queuing;

internal class DomainEventQueuingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<IDomainEventQueue>(provider =>
        {
            IHttpContextAccessor httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            ILogger<HttpContextDomainEventQueueService> logger = provider.GetRequiredService<ILogger<HttpContextDomainEventQueueService>>();
            return new HttpContextDomainEventQueueService(httpContextAccessor, logger);
        });

        // This we have to register as scoped since the HTTP request encapsulates our entire scope.
        services.TryAddScoped<IEnqueueDomainEvents>(provider => provider.GetRequiredService<IDomainEventQueue>());
        services.TryAddScoped<IDequeueDomainEvents>(provider => provider.GetRequiredService<IDomainEventQueue>());
    }
}
