using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Model.Base;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices.Commands.Dispatching;

public class CommandDispatchFacade : ICommandDispatchFacade
{
    private readonly IDispatchCommands _commandDispatcher;
    private readonly IEventCallbackService _eventCallbackService;

    public CommandDispatchFacade(IDispatchCommands commandDispatcher, IEventCallbackService eventCallbackService)
    {
        _commandDispatcher = commandDispatcher;
        _eventCallbackService = eventCallbackService;
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        _eventCallbackService.RegisterEventCallback(action);
    }

    /// <inheritdoc />
    public async Task DispatchAsync(IBaseCommand command, CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.DispatchAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        await _commandDispatcher.DispatchAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand<TResult>
    {
        return await _commandDispatcher.DispatchAsync<TCommand, TResult>(command, cancellationToken);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _eventCallbackService.ClearCallbacks();
    }
}
