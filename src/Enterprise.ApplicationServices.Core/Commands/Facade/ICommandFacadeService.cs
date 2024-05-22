using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.Events.Callbacks.Registration.Abstract;

namespace Enterprise.ApplicationServices.Core.Commands.Facade;

public interface ICommandFacadeService : IDispatchCommands, IRegisterEventCallbacks, IClearCallbacks;
