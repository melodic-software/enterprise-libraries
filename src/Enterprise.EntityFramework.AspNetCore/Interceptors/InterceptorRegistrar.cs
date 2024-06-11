using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Domain.Events.Queuing;
using Enterprise.EntityFramework.AspNetCore.Concurrency;
using Enterprise.EntityFramework.AspNetCore.EventualConsistency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.Interceptors;

internal sealed class InterceptorRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped(_ => new ConcurrencyErrorHandlingInterceptor());

        services.TryAddScoped(provider =>
        {
            ILogger<DomainEventQueuingInterceptor> logger = provider.GetRequiredService<ILogger<DomainEventQueuingInterceptor>>();
            IEnqueueDomainEvents queueService = provider.GetRequiredService<IEnqueueDomainEvents>();
            return new DomainEventQueuingInterceptor(queueService, logger);
        });
    }
}
