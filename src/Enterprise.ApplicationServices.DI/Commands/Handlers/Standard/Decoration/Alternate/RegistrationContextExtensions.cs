using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Alternate.Delegates;
using Enterprise.DI.Core.Registration;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Alternate;

public static class RegistrationContextExtensions
{
    public static RegistrationContext<IHandleCommand<TCommand, TResponse>> WithDecorators<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext,
        params CommandHandlerDecoratorImplementationFactory<TCommand, TResponse>[] decoratorFactories)
        where TCommand : ICommand<TResponse>
    {
        return registrationContext.WithDecorators(decoratorFactories.AsEnumerable());
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> WithDefaultDecorators<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext)
        where TCommand : ICommand<TResponse>
    {
        IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand, TResponse>>
            defaultDecoratorImplementationFactories = CommandHandlerDecoratorImplementationFactories.GetDefault<TCommand, TResponse>();

        return registrationContext.WithDecorators(defaultDecoratorImplementationFactories);
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> RegisterWithDecorators<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext,
        RegistrationOptions<TCommand, TResponse> options)
        where TCommand : ICommand<TResponse>
    {
        registrationContext.AddCommandHandler(options);

        if (options.DecoratorFactories.Any())
        {
            return registrationContext.WithDecorators(options.DecoratorFactories.ToArray());
        }

        registrationContext.WithDefaultDecorators();

        return registrationContext;
    }

    private static RegistrationContext<IHandleCommand<TCommand, TResponse>> WithDecorators<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext,
        IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand, TResponse>> implementationFactories)
        where TCommand : ICommand<TResponse>
    {
        foreach (CommandHandlerDecoratorImplementationFactory<TCommand, TResponse> implementationFactory in implementationFactories)
        {
            registrationContext.WithDecorator(implementationFactory.Invoke);
        }

        return registrationContext;
    }
}
