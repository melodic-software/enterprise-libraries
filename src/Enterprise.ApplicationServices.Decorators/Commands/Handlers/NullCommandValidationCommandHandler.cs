using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers;

public class NullCommandValidationCommandHandler<TCommand> : CommandHandlerDecoratorBase<TCommand>
    where TCommand : class, ICommand
{
    public NullCommandValidationCommandHandler(IHandleCommand<TCommand> commandHandler,
        IGetDecoratedInstance decoratorService)
        : base(commandHandler, decoratorService)
    {

    }

    public override async Task HandleAsync(TCommand? command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        await Decorated.HandleAsync(command, cancellationToken);
    }
}
