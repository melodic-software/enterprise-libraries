using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Domain.AspNetCore.Events.Queuing.Decoration;
using Enterprise.Domain.Events.Queuing;
using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Raising.Callbacks.Raising.Abstract;
using Enterprise.Options.Core.Singleton;
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
        DomainEventQueueConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<DomainEventQueueConfigOptions>(configuration, DomainEventQueueConfigOptions.ConfigSectionKey);

        if (!configOptions.EnableDomainEventQueuing)
            return;

        // This we have to register as scoped since the HTTP request encapsulates our entire scope.
        services.TryAddScoped<IEnqueueDomainEvents>(provider =>
        {
            IHttpContextAccessor httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            ILogger<HttpContextDomainEventQueueService> logger = provider.GetRequiredService<ILogger<HttpContextDomainEventQueueService>>();
            return new HttpContextDomainEventQueueService(httpContextAccessor, logger);
        });

        // We're replacing any existing registrations, and are replacing it with a queue based implementation. 
        services.RemoveAll(typeof(IRaiseDomainEvents));

        // We have to use scoped here because the dependency being injected in is scoped,
        // and would cause problems if we registered this with a singleton lifetime.
        services.BeginRegistration<IRaiseDomainEvents>()
            .AddScoped(provider =>
            {
                IEnqueueDomainEvents domainEventQueueService = provider.GetRequiredService<IEnqueueDomainEvents>();
                return new QueueingDomainEventRaiser(domainEventQueueService);
            })
            .WithDecorator((provider, eventRaiser) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IRaiseEventCallbacks callbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
                return new DomainEventCallbackRaisingDecorator(eventRaiser, decoratorService, callbackRaiser);
            });
    }
}
