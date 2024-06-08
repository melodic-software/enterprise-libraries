﻿using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Types.Alternate;

internal sealed class CommandHandlerTypeService
{
    internal static Type? GetAlternateHandlerType<TCommand>() where TCommand : IBaseCommand
    {
        // Get the type argument of ICommand<>.
        Type? responseType = GetResponseType<TCommand>();

        if (responseType == null)
        {
            return null;
        }

        // Create the generic type for IHandleCommand<TCommand, TResponse>.
        Type handlerType = typeof(IHandleCommand<,>).MakeGenericType(typeof(TCommand), responseType);

        return handlerType;
    }

    internal static Type? GetResponseType<TCommand>() where TCommand : IBaseCommand
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