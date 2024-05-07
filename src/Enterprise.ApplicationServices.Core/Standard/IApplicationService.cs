using Enterprise.Events.Callbacks.Registration.Abstract;

namespace Enterprise.ApplicationServices.Core.Standard;

public interface IApplicationService : IRegisterEventCallbacks, IClearCallbacks;
