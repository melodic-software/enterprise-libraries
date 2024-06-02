using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard;

public static class RegistrationExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand>> RegisterCommandHandler<TCommand>(
        this IServiceCollection services,
        RegistrationOptions<TCommand> options)
        where TCommand : IBaseCommand
    {
        RegistrationContext<IHandleCommand<TCommand>> registration = services
            .BeginRegistration<IHandleCommand<TCommand>>();

        if (options.UseDecorators)
        {
            registration.RegisterWithDecorators(options);
        }
        else
        {
            registration.AddCommandHandler(options);
        }

        return registration;
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> AddCommandHandler<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registration,
        RegistrationOptions<TCommand> options)
        where TCommand : IBaseCommand
    {
        if (options.CommandHandlerFactory == null)
        {
            throw new InvalidOperationException(
                "A command handler factory delegate must be provided for command handler registrations."
            );
        }

        registration.Add(options.CommandHandlerFactory, options.ServiceLifetime);

        return registration;
    }
}
