using Enterprise.ApplicationServices.Core.Commands;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Decorators.CommandHandlers;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Extensions;

public static class CommandHandlerRegistrationExtensions
{
    public static void RegisterCommandHandler<T>(this IServiceCollection services,
        Func<IServiceProvider, IHandleCommand<T>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where T : IBaseCommand
    {
        services.BeginRegistration<IHandleCommand<T>>()
            .Add(factory, serviceLifetime)
            .WithDecorators((provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<T>> validators = provider.GetServices<IValidator<T>>();
                ILogger<FluentValidationCommandHandler<T>> logger = provider.GetRequiredService<ILogger<FluentValidationCommandHandler<T>>>();
                IHandleCommand<T> decorator = new FluentValidationCommandHandler<T>(commandHandler, decoratorService, validators, logger);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IHandleCommand<T> decorator = new NullCommandValidationCommandHandler<T>(commandHandler, decoratorService);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingCommandHandler<T>> logger = provider.GetRequiredService<ILogger<ErrorHandlingCommandHandler<T>>>();
                IHandleCommand<T> decorator = new ErrorHandlingCommandHandler<T>(commandHandler, decoratorService, logger);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingCommandHandler<T>> logger = provider.GetRequiredService<ILogger<LoggingCommandHandler<T>>>();
                IHandleCommand<T> decorator = new LoggingCommandHandler<T>(commandHandler, decoratorService, logger);
                return decorator;
            });
    }
}