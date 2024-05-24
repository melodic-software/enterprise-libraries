using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Raising.Abstract;

namespace Enterprise.Events.Facade.Abstract;

public interface IEventRaisingFacade : IRaiseEvents, IRaiseDomainEvents;
