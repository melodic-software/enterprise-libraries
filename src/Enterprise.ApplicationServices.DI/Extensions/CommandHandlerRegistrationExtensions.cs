using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Extensions;

public static class CommandHandlerRegistrationExtensions
{
    public static void RegisterCommandHandler<TCommand>(this IServiceCollection services,
        Func<IServiceProvider, IHandleCommand<TCommand>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TCommand : ICommand
    {
        services.BeginRegistration<IHandleCommand<TCommand>>()
            .Add(factory, serviceLifetime)
            .WithDecorators((provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TCommand>> validators = provider.GetServices<IValidator<TCommand>>();
                ILogger<FluentValidationCommandHandler<TCommand>> logger = provider.GetRequiredService<ILogger<FluentValidationCommandHandler<TCommand>>>();
                IHandleCommand<TCommand> decorator = new FluentValidationCommandHandler<TCommand>(commandHandler, decoratorService, validators, logger);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IHandleCommand<TCommand> decorator = new NullCommandValidationCommandHandler<TCommand>(commandHandler, decoratorService);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingCommandHandler<TCommand>> logger = provider.GetRequiredService<ILogger<ErrorHandlingCommandHandler<TCommand>>>();
                IHandleCommand<TCommand> decorator = new ErrorHandlingCommandHandler<TCommand>(commandHandler, decoratorService, logger);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingCommandHandler<TCommand>> logger = provider.GetRequiredService<ILogger<LoggingCommandHandler<TCommand>>>();
                IHandleCommand<TCommand> decorator = new LoggingCommandHandler<TCommand>(commandHandler, decoratorService, logger);
                return decorator;
            });
    }

    public static void RegisterCommandHandler<TCommand, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, IHandleCommand<TCommand, TResponse>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TCommand : ICommand<TResponse>
    {
        services.BeginRegistration<IHandleCommand<TCommand, TResponse>>()
            .Add(factory, serviceLifetime)
            .WithDecorators((provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TCommand>> validators = provider.GetServices<IValidator<TCommand>>();
                ILogger<FluentValidationCommandHandler<TCommand, TResponse>> logger = provider.GetRequiredService<ILogger<FluentValidationCommandHandler<TCommand, TResponse>>>();
                IHandleCommand<TCommand, TResponse> decorator = new FluentValidationCommandHandler<TCommand, TResponse>(commandHandler, decoratorService, validators, logger);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IHandleCommand<TCommand, TResponse> decorator = new NullCommandValidationCommandHandler<TCommand, TResponse>(commandHandler, decoratorService);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>> logger = provider.GetRequiredService<ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>>>();
                IHandleCommand<TCommand, TResponse> decorator = new ErrorHandlingCommandHandler<TCommand, TResponse>(commandHandler, decoratorService, logger);
                return decorator;
            }, (provider, commandHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingCommandHandler<TCommand, TResponse>> logger = provider.GetRequiredService<ILogger<LoggingCommandHandler<TCommand, TResponse>>>();
                IHandleCommand<TCommand, TResponse> decorator = new LoggingCommandHandler<TCommand, TResponse>(commandHandler, decoratorService, logger);
                return decorator;
            });
    }
}
