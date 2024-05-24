using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Raising.Abstract;
using Enterprise.Events.Raising.Decorators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising;

internal sealed class EventRaisingServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRecordRaisedEvents>(_ => new RaisedEventRecorder());

        services.BeginRegistration<IRaiseEvents>()
            .TryAddScoped(provider =>
            {
                IDispatchEvents eventDispatcher = provider.GetRequiredService<IDispatchEvents>();
                return new EventRaiser(eventDispatcher);
            })
            .WithDecorators((provider, eventRaiser) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingEventRaiser> logger = provider.GetRequiredService<ILogger<LoggingEventRaiser>>();
                return new LoggingEventRaiser(eventRaiser, decoratorService, logger);
            }, (provider, eventRaiser) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IRecordRaisedEvents raisedEventRecorder = provider.GetRequiredService<IRecordRaisedEvents>();
                ILogger<DuplicatePreventionDecorator> logger = provider.GetRequiredService<ILogger<DuplicatePreventionDecorator>>();
                return new DuplicatePreventionDecorator(eventRaiser, decoratorService, raisedEventRecorder, logger);
            });
    }
}
