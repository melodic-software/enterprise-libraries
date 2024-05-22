using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Model;

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
    public async Task<TResponse?> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        IHandleCommand<TCommand, TResponse> handler = _commandHandlerResolver.GetHandlerFor<TCommand, TResponse>(command);
        TResponse response = await handler.HandleAsync(command, cancellationToken);
        return response;
    }
}
