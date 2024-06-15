using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;

public class LoggingCommandHandler<TCommand> : IHandler<TCommand>
{
    private readonly ILogger<LoggingCommandHandler<TCommand>> _logger;

    public LoggingCommandHandler(ILogger<LoggingCommandHandler<TCommand>> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken = default)
    {
        Type commandType = typeof(TCommand);

        using (_logger.BeginScope("Command: {CommandType}", commandType.Name))
        {
            _logger.LogDebug("Executing command.");
            await next();
            _logger.LogDebug("Command was handled successfully.");
        }
    }
}
