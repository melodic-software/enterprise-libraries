using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;

public class LoggingCommandHandler<TCommand, TResponse> : IHandler<TCommand, TResponse>
{
    private readonly ILogger<LoggingCommandHandler<TCommand, TResponse>> _logger;

    public LoggingCommandHandler(ILogger<LoggingCommandHandler<TCommand, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Type commandType = typeof(TCommand);

        TResponse response;

        using (_logger.BeginScope("Command: {CommandType}", commandType.Name))
        {
            _logger.LogDebug("Executing command.");
            response = await next();
            _logger.LogDebug("Command was handled successfully.");
        }

        return response;
    }
}
