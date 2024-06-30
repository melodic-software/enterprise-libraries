using Enterprise.ApplicationServices.Core.Commands.Model.Base;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommands
{
    Task DispatchAsync(IBaseCommand command, CancellationToken cancellationToken = default);

    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand;

    Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand<TResult>;
}
