using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate;

public class NullCommandValidationCommandHandler<TCommand, TResponse> : CommandHandlerDecoratorBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public NullCommandValidationCommandHandler(IHandleCommand<TCommand, TResponse> commandHandler,
        IGetDecoratedInstance decoratorService)
        : base(commandHandler, decoratorService)
    {

    }

    public override async Task<Result<TResponse>> HandleAsync(TCommand? command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        Result<TResponse> result = await Decorated.HandleAsync(command, cancellationToken);
        return result;
    }
}
