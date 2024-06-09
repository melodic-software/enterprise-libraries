﻿using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;

public class NullCommandValidationCommandHandler<TCommand, TResult> : IHandler<TCommand, TResult>
{
    public async Task<TResult?> HandleAsync(TCommand request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await next();
    }
}
