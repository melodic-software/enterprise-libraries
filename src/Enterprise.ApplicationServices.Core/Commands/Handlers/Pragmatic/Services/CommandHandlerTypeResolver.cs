using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic.Services;

public static class CommandHandlerTypeResolver
{
    public static Type? GetAlternateHandlerType<TCommand>() where TCommand : ICommand
    {
        // Get the type argument of ICommand<>.
        Type? responseType = GeTResultType<TCommand>();

        if (responseType == null)
        {
            return null;
        }

        // Create the generic type for IHandleCommand<TCommand, TResult>.
        Type handlerType = typeof(IHandleCommand<,>).MakeGenericType(typeof(TCommand), responseType);

        return handlerType;
    }

    public static Type? GeTResultType<TCommand>() where TCommand : ICommand
    {
        Type commandType = typeof(TCommand);

        Type? commandInterface = Array.Find(commandType.GetInterfaces(),
            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>)
        );

        if (commandInterface == null)
        {
            return null;
        }

        // Get the type argument of ICommand<>.
        Type responseType = commandInterface.GetGenericArguments().First();

        return responseType;
    }
}
