using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Pragmatic;

public class LoggingCommandHandler<TCommand, TResponse> : CommandHandlerDecoratorBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ILogger<LoggingCommandHandler<TCommand, TResponse>> _logger;

    public LoggingCommandHandler(IHandleCommand<TCommand, TResponse> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingCommandHandler<TCommand, TResponse>> logger)
        : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        Type commandType = typeof(TCommand);
        Type innermostHandlerType = Innermost.GetType();
        
        TResponse response;

        // TODO: Do we want to add a scope (or log statement) that describes the decorator chain?
        // Maybe we do that in the base?

        using (_logger.BeginScope("Command Handler: {CommandHandlerType}, Command: {CommandType}", innermostHandlerType.Name, commandType.Name))
        {
            _logger.LogDebug("Executing command.");
            response = await Decorated.HandleAsync(command, cancellationToken);
            _logger.LogDebug("Command was handled successfully.");
        }

        return response;
    }
}
