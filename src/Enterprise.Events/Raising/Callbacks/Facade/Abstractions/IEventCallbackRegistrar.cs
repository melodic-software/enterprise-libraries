using Enterprise.Events.Raising.Callbacks.Abstractions;

namespace Enterprise.Events.Raising.Callbacks.Facade.Abstractions;

public interface IEventCallbackRegistrar : IRegisterEventCallbacks, IGetRegisteredCallbacks, IClearCallbacks;