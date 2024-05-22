using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Facade;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices.Commands.Facade;

public class CommandFacadeService : ICommandFacadeService
{
    private readonly IDispatchCommands _commandDispatcher;
    private readonly IEventCallbackService _eventService;

    public CommandFacadeService(IDispatchCommands commandDispatcher, IEventCallbackService eventService)
    {
        _commandDispatcher = commandDispatcher;
        _eventService = eventService;
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> action)
        where TEvent : IEvent
    {
        _eventService.RegisterEventCallback(action);
    }

    /// <inheritdoc />
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IBaseCommand
    {
        await _commandDispatcher.DispatchAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        return await _commandDispatcher.DispatchAsync<TCommand, TResponse>(command, cancellationToken);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _eventService.ClearCallbacks();
    }
}
