using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict.NonGeneric;
using Enterprise.ApplicationServices.Core.Commands.Model.Base;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.Commands.Dispatching;

public class CommandDispatcher : IDispatchCommands
{
    private readonly IResolveCommandHandler _commandHandlerResolver;

    public CommandDispatcher(IResolveCommandHandler commandHandlerResolver)
    {
        _commandHandlerResolver = commandHandlerResolver;
    }

    public async Task DispatchAsync(IBaseCommand command, CancellationToken cancellationToken = default)
    {
        IHandleCommand commandHandler = _commandHandlerResolver.GetHandlerFor(command);
        await commandHandler.HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        IHandleCommand<TCommand> handler = _commandHandlerResolver.GetHandlerFor(command);
        await handler.HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand<TResult>
    {
        IHandleCommand<TCommand, TResult> handler = _commandHandlerResolver.GetHandlerFor<TCommand, TResult>(command);
        TResult result = await handler.HandleAsync(command, cancellationToken);
        return result;
    }
}
