using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Alternate;

public static class CommandHandlerDecoratorFactories
{
    public static IEnumerable<Func<IServiceProvider, IHandleCommand<TCommand, TResponse>, IHandleCommand<TCommand, TResponse>>>
        GetDefault<TCommand, TResponse>() where TCommand : ICommand<TResponse>
    {
        return
        [
            (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TCommand>> validators = provider.GetServices<IValidator<TCommand>>();
                ILogger<FluentValidationCommandHandler<TCommand, TResponse>> logger = provider.GetRequiredService<ILogger<FluentValidationCommandHandler<TCommand, TResponse>>>();
                return new FluentValidationCommandHandler<TCommand, TResponse>(commandHandler, decoratorService, validators, logger);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                return new NullCommandValidationCommandHandler<TCommand, TResponse>(commandHandler, decoratorService);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>> logger = provider.GetRequiredService<ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>>>();
                return new ErrorHandlingCommandHandler<TCommand, TResponse>(commandHandler, decoratorService, logger);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingCommandHandler<TCommand, TResponse>> logger = provider.GetRequiredService<ILogger<LoggingCommandHandler<TCommand, TResponse>>>();
                return new LoggingCommandHandler<TCommand, TResponse>(commandHandler, decoratorService, logger);
            }
        ];
    }
}
