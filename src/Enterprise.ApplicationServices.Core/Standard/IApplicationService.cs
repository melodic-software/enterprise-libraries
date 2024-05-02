using Enterprise.Events.Raising.Callbacks.Abstractions;

namespace Enterprise.ApplicationServices.Core.Standard;

public interface IApplicationService : IRegisterEventCallbacks, IClearCallbacks;
