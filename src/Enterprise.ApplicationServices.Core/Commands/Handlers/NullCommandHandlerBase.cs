using Enterprise.ApplicationServices.Core.Commands.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

public abstract class NullCommandHandlerBase
{
    protected ILogger Logger { get; }

    protected NullCommandHandlerBase(ILogger logger)
    {
        Logger = logger;
    }

    protected void LogWarning(ICommand command)
    {
        string handlerTypeName = GetType().Name;
        string commandTypeName = command.GetType().Name;

        Logger.LogWarning(
            "The {HandlerType} has been sent a \"{CommandType}\" command. " +
            "The command will not be handled. " +
            "Please verify command handler registrations.",
            handlerTypeName, commandTypeName
        );
    }
}
