using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Services.Dispatching.Abstract;
using Enterprise.Events.Services.Handlers;
using Enterprise.MediatR.Options;
using Enterprise.Options.Core.Singleton;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Events;

internal class EventServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        MediatRConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MediatRConfigOptions>(configuration, MediatRConfigOptions.ConfigSectionKey);

        if (!options.EnableMediatR)
            return;

        // Clear all existing registrations.
        services.RemoveAll(typeof(IDispatchEvents));

        services.AddSingleton(provider =>
        {
            IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
            IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
            IPublisher publisher = provider.GetRequiredService<IPublisher>();
            ILogger<MediatREventDispatcher> logger = provider.GetRequiredService<ILogger<MediatREventDispatcher>>();

            IDispatchEvents eventDispatcher = new MediatREventDispatcher(publisher, decoratorService, eventHandlerResolver, logger);

            return eventDispatcher;
        });
    }
}