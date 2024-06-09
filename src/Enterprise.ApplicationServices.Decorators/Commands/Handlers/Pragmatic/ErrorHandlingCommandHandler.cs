using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic;

public class ErrorHandlingCommandHandler<TCommand, TResult> : CommandHandlerDecoratorBase<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    private readonly ILogger<ErrorHandlingCommandHandler<TCommand, TResult>> _logger;

    public ErrorHandlingCommandHandler(IHandleCommand<TCommand, TResult> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingCommandHandler<TCommand, TResult>> logger) : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return await Decorated.HandleAsync(command, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the command.");
            throw;
        }
    }
}
