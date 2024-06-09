using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers;

public class ErrorHandlingCommandHandler<TCommand> : CommandHandlerDecoratorBase<TCommand>
    where TCommand : class, ICommand
{
    private readonly ILogger<ErrorHandlingCommandHandler<TCommand>> _logger;

    public ErrorHandlingCommandHandler(IHandleCommand<TCommand> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingCommandHandler<TCommand>> logger) : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await Decorated.HandleAsync(command, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the command.");
            throw;
        }
    }
}
