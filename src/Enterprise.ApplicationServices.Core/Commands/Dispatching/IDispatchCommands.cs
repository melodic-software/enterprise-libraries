using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommands
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IBaseCommand;

    Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>;
}
