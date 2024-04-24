using Enterprise.ApplicationServices.Core.Commands;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Decorators.CommandHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class ErrorHandlingCommandHandler<T> : CommandHandlerDecoratorBase<T>
    where T : IBaseCommand
{
    private readonly ILogger<ErrorHandlingCommandHandler<T>> _logger;

    public ErrorHandlingCommandHandler(IHandleCommand<T> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingCommandHandler<T>> logger) : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override Task HandleAsync(T command)
    {
        try
        {
            return Decorated.HandleAsync(command);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the command.");
            throw;
        }
    }
}