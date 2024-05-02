using Enterprise.DI.Core.Registration;
using Enterprise.EntityFramework.AspNetCore.EventualConsistency;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.Dependencies;

internal class DependencyRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddTransient(provider =>
        {
            IHttpContextAccessor? httpContextAccessor = provider.GetService<IHttpContextAccessor>();
            ILogger<DeferredDomainEventQueueService> logger = provider.GetRequiredService<ILogger<DeferredDomainEventQueueService>>();

            DeferredDomainEventQueueService queueService = new DeferredDomainEventQueueService(httpContextAccessor, logger);

            return queueService;
        });
    }
}
