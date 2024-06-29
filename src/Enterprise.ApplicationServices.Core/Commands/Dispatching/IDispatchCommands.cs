using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommands
{
    Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
