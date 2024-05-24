using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommands
{
    Task DispatchAsync(ICommand command, CancellationToken cancellationToken);

    Task<TResponse?> DispatchAsync<TResponse>(ICommand command, CancellationToken cancellationToken);

    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IBaseCommand;

    Task<TResponse?> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>;
}
