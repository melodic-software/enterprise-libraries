using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic.Services;

public static class CommandHandlerTypeResolver
{
    public static Type? GetAlternateHandlerType<TCommand>() where TCommand : class, ICommand
    {
        // Get the type argument of ICommand<>.
        Type? resultType = GetResultType<TCommand>();

        if (resultType == null)
        {
            return null;
        }

        // Create the generic type for IHandleCommand<TCommand, TResult>.
        Type handlerType = typeof(IHandleCommand<,>).MakeGenericType(typeof(TCommand), resultType);

        return handlerType;
    }

    public static Type? GetResultType<TCommand>() where TCommand : class, ICommand
    {
        Type commandType = typeof(TCommand);

        Type? commandInterface = Array.Find(commandType.GetInterfaces(),
            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>)
        );

        // Get the type argument of ICommand<>.
        Type resultType = commandInterface?.GetGenericArguments().First();

        return resultType;
    }
}
