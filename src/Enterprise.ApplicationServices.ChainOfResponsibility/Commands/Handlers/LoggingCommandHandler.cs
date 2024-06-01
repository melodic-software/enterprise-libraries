using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;

public class LoggingCommandHandler<TCommand, TResponse> : IHandler<TCommand>, IHandler<TCommand, TResponse>
{
    private readonly ILogger<LoggingCommandHandler<TCommand, TResponse>> _logger;

    public LoggingCommandHandler(ILogger<LoggingCommandHandler<TCommand, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken)
    {
        Type commandType = typeof(TCommand);

        using (_logger.BeginScope("Command: {CommandType}", commandType.Name))
        {
            _logger.LogDebug("Executing command.");
            await next();
            _logger.LogDebug("Command was handled successfully.");
        }
    }

    public Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
