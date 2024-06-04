using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Alternate;

public static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> AddCommandHandler<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext,
        RegistrationOptions<TCommand, TResponse> options)
        where TCommand : ICommand<TResponse>
    {
        if (options.CommandHandlerImplementationFactory == null)
        {
            throw new InvalidOperationException(
                "A command handler implementation factory delegate must be provided for command handler registrations."
            );
        }

        registrationContext.Add(options.CommandHandlerImplementationFactory.Invoke, options.ServiceLifetime);

        // We also need to register this as a standard command handler.
        CommandHandlerImplementationFactory<TCommand> standardImplementationFactory =
            options.CommandHandlerImplementationFactory.Invoke;

        var serviceDescriptor = new ServiceDescriptor(
            typeof(IHandleCommand<TCommand>),
            standardImplementationFactory,
            options.ServiceLifetime
        );

        registrationContext.Add(serviceDescriptor);

        return registrationContext;
    }
}
