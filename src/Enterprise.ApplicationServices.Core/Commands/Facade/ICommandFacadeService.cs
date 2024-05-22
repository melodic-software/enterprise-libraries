using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.Events.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices.Core.Commands.Facade;

public interface ICommandFacadeService : IDispatchCommands, IEventCallbackService;
