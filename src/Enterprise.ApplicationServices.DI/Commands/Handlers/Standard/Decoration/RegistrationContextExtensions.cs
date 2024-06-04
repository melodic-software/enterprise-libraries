using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Shared;
using Enterprise.DI.Core.Registration;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration;

public static class RegistrationContextExtensions
{
    public static RegistrationContext<IHandleCommand<TCommand>> WithDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        params CommandHandlerDecoratorImplementationFactory<TCommand>[] decoratorFactories)
        where TCommand : IBaseCommand
    {
        return registrationContext.WithDecorators(decoratorFactories.AsEnumerable());
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> WithDefaultDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext)
        where TCommand : IBaseCommand
    {
        IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand>>
            defaultDecoratorImplementationFactories = CommandHandlerDecoratorImplementationFactories.GetDefault<TCommand>();

        return registrationContext.WithDecorators(defaultDecoratorImplementationFactories);
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> RegisterWithDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        RegistrationOptions<TCommand> options)
        where TCommand : ICommand
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
        where TCommand : IBaseCommand
    {
        foreach (CommandHandlerDecoratorImplementationFactory<TCommand> implementationFactory in implementationFactories)
        {
            registrationContext.WithDecorator(implementationFactory.Invoke);
        }

        return registrationContext;
    }
}
