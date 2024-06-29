using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict.NonGeneric;
using Enterprise.ApplicationServices.Core.Commands.Model.Base;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Validation;

public static class CommandHandlerTypeValidationService
{
    public static void ValidateType<TCommand>(TCommand command, IHandleCommand commandHandler)
        where TCommand : class, IBaseCommand
    {
        ValidateType(command, typeof(TCommand), commandHandler);
    }

    public static void ValidateType<TCommand>(TCommand command, IHandleCommand<TCommand> commandHandler)
        where TCommand : class, ICommand
    {
        ValidateType(command, typeof(TCommand), commandHandler);
    }

    public static void ValidateType(IBaseCommand command, Type expectedCommandType, IHandleCommand commandHandler)
    {
        Type commandType = command.GetType();

        bool commandCanBeHandled = commandType.IsAssignableTo(expectedCommandType);

        if (commandCanBeHandled)
        {
            return;
        }

        Type commandHandlerType = commandHandler.GetType();

        throw new InvalidOperationException(CommandCannotBeHandled(commandType, commandHandlerType));
    }

    private static string CommandCannotBeHandled(Type commandType, Type commandHandlerType) =>
        $"A command of type \"{commandType.FullName}\" cannot be handled by \"{commandHandlerType.FullName}\"";
}
