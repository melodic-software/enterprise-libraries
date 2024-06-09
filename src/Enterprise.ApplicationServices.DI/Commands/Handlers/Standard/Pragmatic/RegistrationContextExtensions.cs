using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Pragmatic;

public static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand, TResult>> AddCommandHandler<TCommand, TResult>(
        this RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext,
        RegistrationOptions<TCommand, TResult> options)
        where TCommand : class, ICommand<TResult>
    {
        if (options.CommandHandlerImplementationFactory == null)
        {
            throw new InvalidOperationException(
                "A command handler implementation factory delegate must be provided for command handler registrations."
            );
        }

        // Register the primary.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleCommand<TCommand, TResult>),
                factory: options.CommandHandlerImplementationFactory.Invoke,
                options.ServiceLifetime)
        );

        // We also need to register this as a standard command handler.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleCommand<TCommand>),
                factory: options.CommandHandlerImplementationFactory.Invoke,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }
}
