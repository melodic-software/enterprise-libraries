using Enterprise.Events.Handlers.Abstract.NonGeneric;
using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Delegates;

public delegate Task<IEnumerable<IHandleEvent>> ResolveEventHandlersAsync(IEvent @event);
