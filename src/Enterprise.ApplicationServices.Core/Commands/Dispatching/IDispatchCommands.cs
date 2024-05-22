using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.Events.Callbacks.Registration.Abstract;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommands : IRegisterEventCallbacks, IClearCallbacks
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IBaseCommand;

    Task<TResponse?> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>;
}
