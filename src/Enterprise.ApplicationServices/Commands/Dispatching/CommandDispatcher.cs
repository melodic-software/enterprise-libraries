using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.Commands.Dispatching;

public class CommandDispatcher : IDispatchCommands
{
    private readonly IResolveCommandHandler _commandHandlerResolver;

    public CommandDispatcher(IResolveCommandHandler commandHandlerResolver)
    {
        _commandHandlerResolver = commandHandlerResolver;
    }

    /// <inheritdoc />
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IBaseCommand
    {
        IHandleCommand<TCommand> handler = _commandHandlerResolver.GetHandlerFor(command);
        await handler.HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>
    {
        IHandleCommand<TCommand, TResult> handler = _commandHandlerResolver.GetHandlerFor<TCommand, TResult>(command);
        TResult result = await handler.HandleAsync(command, cancellationToken);
        return result;
    }
}
