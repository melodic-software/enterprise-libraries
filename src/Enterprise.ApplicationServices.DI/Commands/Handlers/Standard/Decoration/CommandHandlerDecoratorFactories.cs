using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration;

public static class CommandHandlerDecoratorFactories
{
    public static IEnumerable<Func<IServiceProvider, IHandleCommand<TCommand>, IHandleCommand<TCommand>>>
        GetDefault<TCommand>() where TCommand : IBaseCommand
    {
        return
        [
            (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TCommand>> validators = provider.GetServices<IValidator<TCommand>>();
                ILogger<FluentValidationCommandHandler<TCommand>> logger = provider.GetRequiredService<ILogger<FluentValidationCommandHandler<TCommand>>>();
                return new FluentValidationCommandHandler<TCommand>(commandHandler, decoratorService, validators, logger);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                return new NullCommandValidationCommandHandler<TCommand>(commandHandler, decoratorService);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingCommandHandler<TCommand>> logger = provider.GetRequiredService<ILogger<ErrorHandlingCommandHandler<TCommand>>>();
                return new ErrorHandlingCommandHandler<TCommand>(commandHandler, decoratorService, logger);
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingCommandHandler<TCommand>> logger = provider.GetRequiredService<ILogger<LoggingCommandHandler<TCommand>>>();
                return new LoggingCommandHandler<TCommand>(commandHandler, decoratorService, logger);
            }
        ];
    }
}
