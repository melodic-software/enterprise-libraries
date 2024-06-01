using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Dispatching.Decoration;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.MediatR.Options;
using Enterprise.Options.Core.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Events;

internal sealed class EventServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        MediatRConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MediatRConfigOptions>(configuration, MediatRConfigOptions.ConfigSectionKey);

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
                ILogger<CallbackRaisingEventDispatchDecorator> logger = provider.GetRequiredService<ILogger<CallbackRaisingEventDispatchDecorator>>();
                return new CallbackRaisingEventDispatchDecorator(eventDispatcher, decoratorService, eventCallbackRaiser, logger);
            });
    }
}
