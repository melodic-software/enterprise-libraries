using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.Patterns.ResultPattern.Model;

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
        where TCommand : ICommand
    {
        IHandleCommand<TCommand>? handler = _commandHandlerResolver.GetHandlerFor(command);

        if (handler == null)
        {
            throw new InvalidOperationException(
                $"An implementation of {nameof(IHandleCommand<TCommand>)} " +
                $"could not be resolved for command \"{typeof(TCommand).FullName}\"."
            );
        }

        await handler.HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        IHandleCommand<TCommand, TResponse>? handler = _commandHandlerResolver.GetHandlerFor<TCommand, TResponse>(command);

        if (handler == null)
        {
            throw new InvalidOperationException(
                $"An implementation of {nameof(IHandleCommand<TCommand, TResponse>)} " +
                $"could not be resolved for command \"{typeof(TCommand).FullName}\" " +
                $"with \"{typeof(TResponse)}\" response."
            );
        }

        Result<TResponse> response = await handler.HandleAsync(command, cancellationToken);

        return response;
    }
}
