using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers;

public class LoggingCommandHandler<TCommand> : CommandHandlerDecoratorBase<TCommand>
    where TCommand : class, ICommand
{
    private readonly ILogger<LoggingCommandHandler<TCommand>> _logger;

    public LoggingCommandHandler(IHandleCommand<TCommand> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingCommandHandler<TCommand>> logger)
        : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        using (_logger.BeginScope("Command Handler: {@CommandHandler}, Command: {@Command}", Innermost, command))
        {
            _logger.LogDebug("Executing command.");
            await Decorated.HandleAsync(command, cancellationToken);
            _logger.LogDebug("Command was handled successfully.");
        }
    }
}
