using Enterprise.DI.Core.Registration;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Callbacks.Raising.Decoration;
using Enterprise.Events.Callbacks.Registration;
using Enterprise.Events.Callbacks.Registration.Abstract;
using Enterprise.Events.Callbacks.Registration.Decoration;
using Enterprise.Events.Raising.Callbacks.Facade;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Callbacks;

internal class EventCallbackServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.BeginRegistration<IEventCallbackRegistrar>()
            .Add(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<EventCallbackRegistrar>>();
                IEventCallbackRegistrar eventCallbackRegistrar = new EventCallbackRegistrar(logger);
                return eventCallbackRegistrar;
            }, ServiceLifetime.Scoped)
            .WithDecorator((provider, eventCallbackRegistrar) =>
            {
                var logger = provider.GetRequiredService<ILogger<LoggingEventCallbackRegistrar>>();
                IEventCallbackRegistrar decorator = new LoggingEventCallbackRegistrar(eventCallbackRegistrar, logger);
                return decorator;
            });

        services.BeginRegistration<IRaiseEventCallbacks>()
            .Add(provider =>
            {
                var callbackRegistrar = provider.GetRequiredService<IEventCallbackRegistrar>();
                var logger = provider.GetRequiredService<ILogger<EventCallbackRaiser>>();
                var allowMultipleExecutions = false;
                IRaiseEventCallbacks callbackRaiser = new EventCallbackRaiser(callbackRegistrar, logger, allowMultipleExecutions);
                return callbackRaiser;
            }, ServiceLifetime.Scoped)
            .WithDecorator((provider, eventCallbackRaiser) =>
            {
                var logger = provider.GetRequiredService<ILogger<LoggingEventCallbackRaiser>>();
                IRaiseEventCallbacks decorator = new LoggingEventCallbackRaiser(eventCallbackRaiser, logger);
                return decorator;
            });

        services.AddScoped<IEventCallbackService>(provider =>
        {
            var callbackRegistrar = provider.GetRequiredService<IEventCallbackRegistrar>();
            var callbackRaiser = provider.GetRequiredService<IRaiseEventCallbacks>();
            return new EventCallbackService(callbackRegistrar, callbackRaiser);
        });
    }
}
