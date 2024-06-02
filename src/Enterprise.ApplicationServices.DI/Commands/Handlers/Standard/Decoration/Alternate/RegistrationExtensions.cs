using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Alternate;
using Enterprise.DI.Core.Registration;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Alternate;

public static class RegistrationExtensions
{
    public static void WithDecorators<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext,
        params Func<IServiceProvider, IHandleCommand<TCommand, TResponse>, IHandleCommand<TCommand, TResponse>>[] decoratorFactories)
        where TCommand : ICommand<TResponse>
    {
        registrationContext.WithDecorators(decoratorFactories);
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> WithDefaultDecorators<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registrationContext)
        where TCommand : ICommand<TResponse>
    {
        IEnumerable<Func<IServiceProvider, IHandleCommand<TCommand, TResponse>, IHandleCommand<TCommand, TResponse>>>
            decoratorFactories = CommandHandlerDecoratorFactories.GetDefault<TCommand, TResponse>();

        return registrationContext.WithDecorators(decoratorFactories.ToArray());
    }

    internal static RegistrationContext<IHandleCommand<TCommand, TResponse>> RegisterWithDecorators<TCommand, TResponse>(
        this RegistrationContext<IHandleCommand<TCommand, TResponse>> registration,
        RegistrationOptions<TCommand, TResponse> options)
        where TCommand : ICommand<TResponse>
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
