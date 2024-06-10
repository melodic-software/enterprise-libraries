﻿using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DI.Core.Registration.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Shared;

internal static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleCommand<TCommand>> AddCommandHandler<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        RegistrationOptions<TCommand> options)
        where TCommand : class, ICommand
    {
        if (options.CommandHandlerImplementationFactory == null)
        {
            throw new InvalidOperationException(
                "A command handler implementation factory delegate must be provided for command handler registrations."
            );
        }

        // Add primary registration.
        registrationContext.Add(
            ServiceDescriptor.Describe(
                typeof(IHandleCommand<TCommand>),
                options.CommandHandlerImplementationFactory.Invoke,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }
}
