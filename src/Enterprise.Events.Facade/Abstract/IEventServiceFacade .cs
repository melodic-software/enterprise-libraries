using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Raising.Abstract;

namespace Enterprise.Events.Facade.Abstract;

public interface IEventServiceFacade : IRaiseEvents, IRaiseDomainEvents, IEventCallbackService;
