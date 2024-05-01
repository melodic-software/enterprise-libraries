using Enterprise.DI.Core.Registration;
using Enterprise.EntityFramework.AspNetCore.Concurrency;
using Enterprise.EntityFramework.AspNetCore.EventualConsistency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.Dependencies;

public class InterceptorRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped(_ => new ConcurrencyErrorHandlingInterceptor());

        services.TryAddScoped(provider =>
        {
            ILogger<DeferredDomainEventInterceptor> logger = provider.GetRequiredService<ILogger<DeferredDomainEventInterceptor>>();
            DeferredDomainEventQueueService queueService = provider.GetRequiredService<DeferredDomainEventQueueService>();

            return new DeferredDomainEventInterceptor(logger, queueService);
        });
    }
}
