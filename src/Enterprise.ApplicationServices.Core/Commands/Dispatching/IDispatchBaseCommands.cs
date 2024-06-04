using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchBaseCommands
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IBaseCommand;
}
