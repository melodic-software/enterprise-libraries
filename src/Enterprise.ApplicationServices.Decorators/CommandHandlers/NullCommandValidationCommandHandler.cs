using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Decorators.CommandHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class NullCommandValidationCommandHandler<T> : CommandHandlerDecoratorBase<T>
    where T : ICommand
{
    public NullCommandValidationCommandHandler(IHandleCommand<T> commandHandler, IGetDecoratedInstance decoratorService) : base(commandHandler, decoratorService)
    {

    }

    public override Task HandleAsync(T? command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return Decorated.HandleAsync(command, cancellationToken);
    }
}
