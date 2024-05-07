using Enterprise.Events.Raising.Callbacks.Raising.Abstract;
using Enterprise.Events.Raising.Callbacks.Registration.Abstract;

namespace Enterprise.Events.Raising.Callbacks.Facade.Abstractions;

public interface IEventCallbackService : IRegisterEventCallbacks, IGetRegisteredCallbacks, IRaiseEventCallbacks, IClearCallbacks;
