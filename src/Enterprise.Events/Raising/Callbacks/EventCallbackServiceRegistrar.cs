using Enterprise.DI.Core.Registration;
using Enterprise.Events.Raising.Callbacks.Facade;
using Enterprise.Events.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Raising.Callbacks.Raising.Abstract;
using Enterprise.Events.Raising.Callbacks.Raising.Decoration;
using Enterprise.Events.Raising.Callbacks.Registration;
using Enterprise.Events.Raising.Callbacks.Registration.Abstract;
using Enterprise.Events.Raising.Callbacks.Registration.Decoration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Callbacks;

internal class EventCallbackServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.BeginRegistration<IEventCallbackRegistrar>()
            .Add(provider =>
            {
                ILogger<EventCallbackRegistrar> logger = provider.GetRequiredService<ILogger<EventCallbackRegistrar>>();
                IEventCallbackRegistrar eventCallbackRegistrar = new EventCallbackRegistrar(logger);
                return eventCallbackRegistrar;
            }, ServiceLifetime.Scoped)
            .WithDecorator((provider, eventCallbackRegistrar) =>
            {
                ILogger<LoggingEventCallbackRegistrar> logger = provider.GetRequiredService<ILogger<LoggingEventCallbackRegistrar>>();
                IEventCallbackRegistrar decorator = new LoggingEventCallbackRegistrar(eventCallbackRegistrar, logger);
                return decorator;
            });

        services.BeginRegistration<IRaiseEventCallbacks>()
            .Add(provider =>
            {
                IEventCallbackRegistrar callbackRegistrar = provider.GetRequiredService<IEventCallbackRegistrar>();
                ILogger<EventCallbackRaiser> logger = provider.GetRequiredService<ILogger<EventCallbackRaiser>>();
                bool allowMultipleExecutions = false;
                IRaiseEventCallbacks callbackRaiser = new EventCallbackRaiser(callbackRegistrar, logger, allowMultipleExecutions);
                return callbackRaiser;
            }, ServiceLifetime.Scoped)
            .WithDecorator((provider, eventCallbackRaiser) =>
            {
                ILogger<LoggingEventCallbackRaiser> logger = provider.GetRequiredService<ILogger<LoggingEventCallbackRaiser>>();
                IRaiseEventCallbacks decorator = new LoggingEventCallbackRaiser(eventCallbackRaiser, logger);
                return decorator;
            });

        services.AddScoped<IEventCallbackService>(provider =>
        {
            IEventCallbackRegistrar callbackRegistrar = provider.GetRequiredService<IEventCallbackRegistrar>();
            IRaiseEventCallbacks callbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
            return new EventCallbackService(callbackRegistrar, callbackRaiser);
        });
    }
}
