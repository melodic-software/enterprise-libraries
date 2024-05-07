using Enterprise.Events.Callbacks.Raising.Abstract;
using Enterprise.Events.Callbacks.Registration.Abstract;

namespace Enterprise.Events.Callbacks.Facade.Abstractions;

public interface IEventCallbackService : IRegisterEventCallbacks, IGetRegisteredCallbacks, IRaiseEventCallbacks, IClearCallbacks;
