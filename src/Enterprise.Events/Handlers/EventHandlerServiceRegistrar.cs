using Enterprise.DI.Core.Registration;
using Enterprise.Events.Handlers.Resolution;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Handlers.Resolution.Decoration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Handlers;

internal sealed class EventHandlerServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Do we want to add configuration to dynamically choose between these registrations?
        // The less complex and reasonably performant resolver is going to be the dynamic dispatch implementation
        // We could also go with the implementation that uses reflection - the initial performance hit is heavier, but the caching mitigates that.

        RegistrationContext<IResolveEventHandlers> registrationContext = services
            .BeginRegistration<IResolveEventHandlers>()
            .TryAddSingleton(provider => new DynamicDispatchingEventHandlerResolver(provider));

        //registrationContext = services
        //    .BeginRegistration<IResolveEventHandlers>()
        //    .TryAddSingleton(provider => new ReflectionEventHandlerResolver(provider));
        // We add decorators that will improve performance and observability.

        registrationContext
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
