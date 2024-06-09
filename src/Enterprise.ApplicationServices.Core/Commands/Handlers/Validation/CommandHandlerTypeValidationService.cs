using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Validation;

public static class CommandHandlerTypeValidationService
{
    public static void ValidateType<TCommand>(IBaseCommand command, IHandleCommand commandHandler)
        where TCommand : IBaseCommand
    {
        ValidateType(command, typeof(TCommand), commandHandler);
    }

    public static void ValidateType<TCommand>(IBaseCommand command, IHandleCommand<TCommand> commandHandler)
        where TCommand : class, IBaseCommand
    {
        ValidateType(command, typeof(TCommand), commandHandler);
    }

    public static void ValidateType(IBaseCommand command, Type expectedCommandType, IHandleCommand commandHandler)
    {
        Type commandType = command.GetType();

        bool commandCanBeHandled = commandType.IsAssignableFrom(expectedCommandType);

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
