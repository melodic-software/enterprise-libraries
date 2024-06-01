using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Decorators.Commands.Handlers.Alternate;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Commands.Alternate;

public static class CommandHandlerRegistrationExtensions
{
    public static void RegisterCommandHandler<TCommand, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, IHandleCommand<TCommand, TResponse>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TCommand : ICommand<TResponse>
    {
        services.BeginRegistration<IHandleCommand<TCommand, TResponse>>()
            .Add(factory, serviceLifetime)
            .WithDefaultDecorators();
    }

    public static void RegisterCommandHandler<TCommand, TResponse, TImplementation>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TImplementation : class, IHandleCommand<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        services.BeginRegistration<IHandleCommand<TCommand, TResponse>>()
            .Add<TImplementation>(serviceLifetime)
            .WithDefaultDecorators();
    }

    private static void WithDefaultDecorators<TCommand, TResponse>(this RegistrationContext<IHandleCommand<TCommand, TResponse>> registration)
        where TCommand : ICommand<TResponse>
    {
        registration
            .WithDecorators((provider, commandHandler) =>
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
            });
    }
}
