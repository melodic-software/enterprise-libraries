using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic;

public class NullCommandValidationCommandHandler<TCommand, TResponse> : CommandHandlerDecoratorBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public NullCommandValidationCommandHandler(IHandleCommand<TCommand, TResponse> commandHandler,
        IGetDecoratedInstance decoratorService)
        : base(commandHandler, decoratorService)
    {

    }

    public override async Task<TResponse> HandleAsync(TCommand? command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        TResponse response = await Decorated.HandleAsync(command, cancellationToken);
        return response;
    }
}
