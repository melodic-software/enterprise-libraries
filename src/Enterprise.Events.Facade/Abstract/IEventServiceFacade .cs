using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Raising.Abstract;
using Enterprise.Events.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.Events.Facade.Abstract;

public interface IEventServiceFacade : IRaiseEvents, IRaiseDomainEvents, IEventCallbackService;
