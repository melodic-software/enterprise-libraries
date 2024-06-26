﻿using Enterprise.ApplicationServices.Core.Commands;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Decorators.CommandHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class LoggingCommandHandler<T> : CommandHandlerDecoratorBase<T>
    where T : IBaseCommand
{
    private readonly ILogger<LoggingCommandHandler<T>> _logger;

    public LoggingCommandHandler(IHandleCommand<T> commandHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<LoggingCommandHandler<T>> logger)
        : base(commandHandler, decoratorService)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(T command)
    {
        Type commandType = typeof(T);
        Type innermostHandlerType = InnermostHandler.GetType();

        // TODO: Do we want to add a scope (or log statement) that describes the decorator chain?
        // Maybe we do that in the base?

        using (_logger.BeginScope("Command Handler: {CommandHandlerType}, Command: {CommandType}", innermostHandlerType.Name, commandType.Name))
        {
            _logger.LogDebug("Executing command.");
            await Decorated.HandleAsync(command);
            _logger.LogDebug("Command was handled successfully.");
        }
    }
}