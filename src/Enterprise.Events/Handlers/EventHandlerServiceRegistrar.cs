using Enterprise.DI.Core.Registration;
using Enterprise.Events.Handlers.Resolution;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Handlers.Resolution.Decoration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Handlers;

internal class EventHandlerServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.BeginRegistration<IResolveEventHandlers>()
            .AddSingleton(provider => new ReflectionEventHandlerResolver(provider))
            .WithDecorators((provider, eventHandlerResolver) =>
            {
                ILogger<CachingEventHandlerResolver> logger = provider.GetRequiredService<ILogger<CachingEventHandlerResolver>>();
                return new CachingEventHandlerResolver(eventHandlerResolver, logger);
            }, (provider, eventHandlerResolver) =>
            {
                ILogger<LoggingEventHandlerResolver> logger = provider.GetRequiredService<ILogger<LoggingEventHandlerResolver>>();
                return new LoggingEventHandlerResolver(eventHandlerResolver, logger);
            });
    }
}
