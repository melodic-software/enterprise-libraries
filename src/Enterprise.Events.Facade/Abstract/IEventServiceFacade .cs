using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Services.Raising.Abstract;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.Events.Facade.Abstract;

public interface IEventServiceFacade : IRaiseEvents, IRaiseDomainEvents, IEventCallbackService;