using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration.Context.Extensions;
using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Decoration;
using Enterprise.Events.Handlers.Delegates;
using Enterprise.Events.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Handlers.Dependencies;

public static class EventHandlerRegistrar
{
    public static void RegisterEventHandler<T>(this IServiceCollection services,
        EventHandlerImplementationFactory<T> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where T : IEvent
    {
        services.BeginRegistration<IHandleEvent<T>>()
            .Add(factory.Invoke, serviceLifetime)
            .WithDecorators((provider, eventHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                return new NullEventValidationEventHandler<T>(eventHandler, decoratorService);
            }, (provider, eventHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingEventHandler<T>> logger = provider.GetRequiredService<ILogger<ErrorHandlingEventHandler<T>>>();
                return new ErrorHandlingEventHandler<T>(eventHandler, decoratorService, logger);
            }, (provider, eventHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingEventHandler<T>> logger = provider.GetRequiredService<ILogger<LoggingEventHandler<T>>>();
                return new LoggingEventHandler<T>(eventHandler, decoratorService, logger);
            });
    }
}
