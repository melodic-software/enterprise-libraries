using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Pragmatic.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Pragmatic;
using Enterprise.DI.Core.Registration.Context;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Pragmatic;

public static class RegistrationContextExtensions
{
    public static RegistrationContext<IHandleCommand<TCommand, TResult>> WithDecorators<TCommand, TResult>(
        this RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext,
        params CommandHandlerDecoratorImplementationFactory<TCommand, TResult>[] decoratorFactories)
        where TCommand : class, ICommand<TResult>
    {
        return registrationContext.WithDecorators(decoratorFactories.AsEnumerable());
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResult>> WithDefaultDecorators<TCommand, TResult>(
        this RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext)
        where TCommand : class, ICommand<TResult>
    {
        IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand, TResult>>
            defaultDecoratorImplementationFactories = CommandHandlerDecoratorImplementationFactories.GetDefault<TCommand, TResult>();

        return registrationContext.WithDecorators(defaultDecoratorImplementationFactories);
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResult>> RegisterWithDecorators<TCommand, TResult>(
        this RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext,
        RegistrationOptions<TCommand, TResult> options)
        where TCommand : class, ICommand<TResult>
    {
        registrationContext.AddCommandHandler(options);

        if (options.DecoratorFactories.Any())
        {
            return registrationContext.WithDecorators(options.DecoratorFactories.ToArray());
        }

        registrationContext.WithDefaultDecorators();

        return registrationContext;
    }

    private static RegistrationContext<IHandleCommand<TCommand, TResult>> WithDecorators<TCommand, TResult>(
        this RegistrationContext<IHandleCommand<TCommand, TResult>> registrationContext,
        IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand, TResult>> implementationFactories)
        where TCommand : class, ICommand<TResult>
    {
        foreach (CommandHandlerDecoratorImplementationFactory<TCommand, TResult> implementationFactory in implementationFactories)
        {
            registrationContext.WithDecorator(implementationFactory.Invoke);
        }

        return registrationContext;
    }
}
