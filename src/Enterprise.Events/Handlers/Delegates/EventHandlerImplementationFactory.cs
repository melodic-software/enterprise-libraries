using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Delegates;

public delegate IHandleEvent<T> EventHandlerImplementationFactory<in T>(IServiceProvider provider) where T : IEvent;
