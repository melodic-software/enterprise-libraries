using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;

public class ErrorHandlingCommandHandler<TCommand> : IHandler<TCommand>
{
    private readonly ILogger<ErrorHandlingCommandHandler<TCommand>> _logger;

    public ErrorHandlingCommandHandler(ILogger<ErrorHandlingCommandHandler<TCommand>> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken = default)
    {
        try
        {
            await next();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the command.");
            throw;
        }
    }
}
