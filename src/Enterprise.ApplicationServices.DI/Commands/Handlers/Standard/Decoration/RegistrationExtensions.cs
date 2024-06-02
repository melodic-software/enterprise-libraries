using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DI.Core.Registration;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration;

public static class RegistrationExtensions
{
    public static void WithDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext,
        params Func<IServiceProvider, IHandleCommand<TCommand>, IHandleCommand<TCommand>>[] decoratorFactories)
        where TCommand : IBaseCommand
    {
        registrationContext.WithDecorators(decoratorFactories);
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> WithDefaultDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registrationContext)
        where TCommand : IBaseCommand
    {
        IEnumerable<Func<IServiceProvider, IHandleCommand<TCommand>, IHandleCommand<TCommand>>>
            decoratorFactories = CommandHandlerDecoratorFactories.GetDefault<TCommand>();

        return registrationContext.WithDecorators(decoratorFactories.ToArray());
    }

    internal static RegistrationContext<IHandleCommand<TCommand>> RegisterWithDecorators<TCommand>(
        this RegistrationContext<IHandleCommand<TCommand>> registration,
        RegistrationOptions<TCommand> options)
        where TCommand : IBaseCommand
    {
        registration.AddCommandHandler(options);

        if (options.DecoratorFactories.Any())
        {
            registration.WithDecorators(options.DecoratorFactories.ToArray());
        }
        else
        {
            registration.WithDefaultDecorators();
        }

        return registration;
    }
}
