using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate;

public class ErrorHandlingCommandHandler<TCommand, TResponse> : CommandHandlerDecoratorBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>> _logger;

    public ErrorHandlingCommandHandler(IHandleCommand<TCommand, TResponse> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>> logger) : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            return await Decorated.HandleAsync(command, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the command.");
            throw;
        }
    }
}
