using Enterprise.Events.Callbacks.Registration.Abstract;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface ICommandDispatchFacade : IDispatchCommands, IRegisterEventCallbacks, IClearCallbacks;
