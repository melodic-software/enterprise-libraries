﻿using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Commands.Handlers;

public class NullCommandHandler<TCommand> : NullCommandHandlerBase, IHandleCommand<TCommand>
    where TCommand : ICommand
{
    public NullCommandHandler(ILogger<NullCommandHandler<TCommand>> logger) : base(logger)
    {

    }

    public Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        LogWarning(command);
        return Task.CompletedTask;
    }

    public Task HandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        LogWarning(command);
        return Task.CompletedTask;
    }
}