using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
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
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IBaseCommand
    {
        await _commandDispatcher.DispatchAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        return await _commandDispatcher.DispatchAsync<TCommand, TResponse>(command, cancellationToken);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _eventCallbackService.ClearCallbacks();
    }
}
