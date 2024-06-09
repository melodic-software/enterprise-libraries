using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;

public class LoggingCommandHandler<TCommand, TResult> : IHandler<TCommand, TResult>
{
    private readonly ILogger<LoggingCommandHandler<TCommand, TResult>> _logger;

    public LoggingCommandHandler(ILogger<LoggingCommandHandler<TCommand, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult?> HandleAsync(TCommand request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken)
    {
        Type commandType = typeof(TCommand);

        TResult response;

        using (_logger.BeginScope("Command: {CommandType}", commandType.Name))
        {
            _logger.LogDebug("Executing command.");
            response = await next();
            _logger.LogDebug("Command was handled successfully.");
        }

        return response;
    }
}
