using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic;

public class LoggingCommandHandler<TCommand, TResult> : CommandHandlerDecoratorBase<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    private readonly ILogger<LoggingCommandHandler<TCommand, TResult>> _logger;

    public LoggingCommandHandler(IHandleCommand<TCommand, TResult> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingCommandHandler<TCommand, TResult>> logger)
        : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        Type commandType = typeof(TCommand);
        Type innermostHandlerType = Innermost.GetType();
        
        TResult result;

        // TODO: Do we want to add a scope (or log statement) that describes the decorator chain?
        // Maybe we do that in the base?

        using (_logger.BeginScope("Command Handler: {CommandHandlerType}, Command: {CommandType}", innermostHandlerType.Name, commandType.Name))
        {
            _logger.LogDebug("Executing command.");
            result = await Decorated.HandleAsync(command, cancellationToken);
            _logger.LogDebug("Command was handled successfully.");
        }

        return result;
    }
}
