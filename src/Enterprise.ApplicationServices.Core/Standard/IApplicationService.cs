using Enterprise.Events.Services.Raising.Callbacks.Abstractions;

namespace Enterprise.ApplicationServices.Core.Standard;

public interface IApplicationService : IRegisterEventCallbacks, IClearCallbacks;