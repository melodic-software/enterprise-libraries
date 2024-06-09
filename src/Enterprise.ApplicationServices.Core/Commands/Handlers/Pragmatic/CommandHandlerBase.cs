﻿using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Library.Core.Attributes;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;

/// <summary>
/// A pragmatic base implementation of a command handler that returns a response.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
[AlternativeTo(typeof(CommandHandlerBase<>))]
public abstract class CommandHandlerBase<TCommand, TResponse>
    : ApplicationServiceBase, IHandleCommand<TCommand, TResponse>, IHandler<TCommand, TResponse>
    where TCommand : class, ICommand<TResponse>
{
    protected CommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    async Task IHandleCommand<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
