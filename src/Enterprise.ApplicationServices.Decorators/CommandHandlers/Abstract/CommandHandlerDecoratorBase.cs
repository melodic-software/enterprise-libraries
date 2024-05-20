using Enterprise.ApplicationServices.Core.Commands;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers.Abstract;

public abstract class CommandHandlerDecoratorBase<T> : DecoratorBase<IHandleCommand<T>>, IHandleCommand<T>
    where T : IBaseCommand
{
    
    protected CommandHandlerDecoratorBase(IHandleCommand<T> commandHandler, IGetDecoratedInstance decoratorService)
        : base(commandHandler, decoratorService)
    {
    }

    /// <inheritdoc />
    public Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (T)command;
        return HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(T command, CancellationToken cancellationToken);

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        Decorated.ClearCallbacks();
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        Decorated.RegisterEventCallback(action);
    }
}
