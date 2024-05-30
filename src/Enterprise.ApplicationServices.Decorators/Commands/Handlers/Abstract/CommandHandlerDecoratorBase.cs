using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Abstract;

public abstract class CommandHandlerDecoratorBase<TCommand> : DecoratorBase<IHandleCommand<TCommand>>, IHandleCommand<TCommand>
    where TCommand : IBaseCommand
{

    protected CommandHandlerDecoratorBase(IHandleCommand<TCommand> commandHandler, IGetDecoratedInstance decoratorService)
        : base(commandHandler, decoratorService)
    {
    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
