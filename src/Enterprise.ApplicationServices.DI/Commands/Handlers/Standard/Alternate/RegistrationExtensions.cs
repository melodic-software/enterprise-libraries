using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Alternate;

public static class RegistrationExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> RegisterCommandHandler<TCommand, TResponse>(
        this IServiceCollection services,
        RegistrationOptions<TCommand, TResponse> options)
        where TCommand : ICommand<TResponse>
    {
        RegistrationContext<IHandleCommand<TCommand, TResponse>> registration = services
            .BeginRegistration<IHandleCommand<TCommand, TResponse>>();

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

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> AddCommandHandler<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registration,
        RegistrationOptions<TCommand, TResponse> options)
        where TCommand : ICommand<TResponse>
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
