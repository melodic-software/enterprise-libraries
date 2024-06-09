using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic.Abstract;

public abstract class CommandHandlerDecoratorBase<TCommand, TResult> :
    DecoratorBase<IHandleCommand<TCommand, TResult>>, IHandleCommand<TCommand, TResult>
    where TCommand : ICommand<TResult>
{

    protected CommandHandlerDecoratorBase(IHandleCommand<TCommand, TResult> commandHandler, IGetDecoratedInstance decoratorService)
        : base(commandHandler, decoratorService)
    {
    }

    /// <inheritdoc />
    async Task IHandleCommand<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
