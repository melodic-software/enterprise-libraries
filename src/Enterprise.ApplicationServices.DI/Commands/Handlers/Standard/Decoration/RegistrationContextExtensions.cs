using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Shared;
using Enterprise.DI.Core.Registration.Context;
using Enterprise.DI.Core.Registration.Context.Delegates;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration;

public static class RegistrationContextExtensions
{
    public static RegistrationContext<IHandleCommand<TCommand>> WithDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        params CommandHandlerDecoratorImplementationFactory<TCommand>[] decoratorFactories)
        where TCommand : class, ICommand
    {
        return registrationContext.WithDecorators(decoratorFactories.AsEnumerable());
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> WithDefaultDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext)
        where TCommand : class, ICommand
    {
        IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand>>
            defaultDecoratorImplementationFactories = CommandHandlerDecoratorImplementationFactories.GetDefault<TCommand>();

        return registrationContext.WithDecorators(defaultDecoratorImplementationFactories);
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> RegisterWithDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        RegistrationOptions<TCommand> options)
        where TCommand : class, ICommand
    {
        registrationContext.AddCommandHandler(options);

        if (options.DecoratorFactories.Any())
        {
            return registrationContext.WithDecorators(options.DecoratorFactories.ToArray());
        }

        return registrationContext.WithDefaultDecorators();
    }

    private static RegistrationContext<IHandleCommand<TCommand>> WithDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand>> implementationFactories)
        where TCommand : class, ICommand
    {
        DecoratorFactory<IHandleCommand<TCommand>>[] decoratorFactories = implementationFactories
            .Select(implementationFactory =>
                new DecoratorFactory<IHandleCommand<TCommand>>((provider, service) =>
                    implementationFactory(provider, service))
            )
            .ToArray();

        registrationContext.WithDecorators(decoratorFactories);

        return registrationContext;
    }
}
