using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;

public class ErrorHandlingCommandHandler<TCommand, TResult> : IHandler<TCommand, TResult>
{
    private readonly ILogger<ErrorHandlingCommandHandler<TCommand, TResult>> _logger;

    public ErrorHandlingCommandHandler(ILogger<ErrorHandlingCommandHandler<TCommand, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult?> HandleAsync(TCommand request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the command.");
            throw;
        }
    }
}
