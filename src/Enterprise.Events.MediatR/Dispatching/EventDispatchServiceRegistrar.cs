using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.DI.Registration.Context.Extensions;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Dispatching.Decoration;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.MediatR.Options;
using Enterprise.Options.Core.Services.Singleton;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.MediatR.Dispatching;

internal sealed class EventDispatchServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        MediatROptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MediatROptions>(configuration, MediatROptions.ConfigSectionKey);

        if (!options.EnableMediatR)
        {
            return;
        }

        // Clear all existing registrations.
        services.RemoveAll(typeof(IDispatchEvents));

        // This has to be a scoped registration because IRaiseEventCallbacks uses a scoped lifetime.
        services.BeginRegistration<IDispatchEvents>()
            .TryAddScoped(provider =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
                IPublisher publisher = provider.GetRequiredService<IPublisher>();
                ILogger<MediatREventDispatcher> logger = provider.GetRequiredService<ILogger<MediatREventDispatcher>>();
                IDispatchEvents eventDispatcher = new MediatREventDispatcher(publisher, decoratorService, eventHandlerResolver, logger);
                return eventDispatcher;
            }).WithDecorator((provider, eventDispatcher) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IRaiseEventCallbacks eventCallbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
                return new CallbackRaisingEventDispatchDecorator(eventDispatcher, decoratorService, eventCallbackRaiser);
            });
    }
}
