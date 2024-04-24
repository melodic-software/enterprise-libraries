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
    public Task HandleAsync(IBaseCommand command)
    {
        ValidateType(command, this);
        T typedCommand = (T)command;
        return HandleAsync(typedCommand);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(T command);

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        Decorated.ClearCallbacks();
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        Decorated.RegisterEventCallback(eventCallback);
    }
}