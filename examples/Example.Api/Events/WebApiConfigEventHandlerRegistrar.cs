using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.Events.Dispatching.Abstract;
using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Abstract.NonGeneric;

namespace Example.Api.Events;

public class WebApiConfigEventHandlerRegistrar : IRegisterWebApiConfigEventHandlers
{
    public static void RegisterHandlers(WebApiConfigEvents events)
    {
        events.WebApplicationBuilt += async app =>
        {
            await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
            IDispatchEvents eventDispatcher = scope.ServiceProvider.GetRequiredService<IDispatchEvents>();

            var @event = new MyEvent();

            await eventDispatcher.DispatchAsync(@event);

            IEnumerable<IHandleEvent<MyEvent>> eventHandlers = app.Services.GetServices<IHandleEvent<MyEvent>>();
            IEnumerable<IHandleEvent> nonGenericEventHandlers = app.Services.GetServices<IHandleEvent>();
        };
    }
}
