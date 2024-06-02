﻿using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Alternate;

public class ErrorHandlingCommandHandler<TCommand, TResponse> : IHandler<TCommand, TResponse>
{
    private readonly ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>> _logger;

    public ErrorHandlingCommandHandler(ILogger<ErrorHandlingCommandHandler<TCommand, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while handling the command.");
            throw;
        }
    }
}