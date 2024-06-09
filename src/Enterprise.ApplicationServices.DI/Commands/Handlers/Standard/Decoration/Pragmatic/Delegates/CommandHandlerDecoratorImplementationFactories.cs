using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Pragmatic.Delegates;

public static class CommandHandlerDecoratorImplementationFactories
{
    public static IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand, TResult>>
        GetDefault<TCommand, TResult>() where TCommand : ICommand<TResult>
    {
        return
        [
            (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TCommand>> validators = provider.GetServices<IValidator<TCommand>>();
                ILogger<FluentValidationCommandHandler<TCommand, TResult>> logger = provider.GetRequiredService<ILogger<FluentValidationCommandHandler<TCommand, TResult>>>();
                return new FluentValidationCommandHandler<TCommand, TResult>(commandHandler, decoratorService, validators, logger);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                return new NullCommandValidationCommandHandler<TCommand, TResult>(commandHandler, decoratorService);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingCommandHandler<TCommand, TResult>> logger = provider.GetRequiredService<ILogger<ErrorHandlingCommandHandler<TCommand, TResult>>>();
                return new ErrorHandlingCommandHandler<TCommand, TResult>(commandHandler, decoratorService, logger);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingCommandHandler<TCommand, TResult>> logger = provider.GetRequiredService<ILogger<LoggingCommandHandler<TCommand, TResult>>>();
                return new LoggingCommandHandler<TCommand, TResult>(commandHandler, decoratorService, logger);
            }
        ];
    }
}
