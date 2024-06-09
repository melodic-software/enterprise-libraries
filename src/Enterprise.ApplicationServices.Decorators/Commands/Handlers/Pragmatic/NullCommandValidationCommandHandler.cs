using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic;

public class NullCommandValidationCommandHandler<TCommand, TResult> : CommandHandlerDecoratorBase<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public NullCommandValidationCommandHandler(IHandleCommand<TCommand, TResult> commandHandler,
        IGetDecoratedInstance decoratorService)
        : base(commandHandler, decoratorService)
    {

    }

    public override async Task<TResult> HandleAsync(TCommand? command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        TResult response = await Decorated.HandleAsync(command, cancellationToken);
        return response;
    }
}
