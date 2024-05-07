using Enterprise.Events.Raising.Callbacks.Registration.Abstract;

namespace Enterprise.ApplicationServices.Core.Standard;

public interface IApplicationService : IRegisterEventCallbacks, IClearCallbacks;
