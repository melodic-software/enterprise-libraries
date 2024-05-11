﻿using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Example.WebApi.ChainOfResponsibility.Examples.Pipeline.Handlers;

public class NonMatchingPipelineHandler : IHandler<string, bool>
{
    public Task<bool> HandleAsync(string request, SuccessorDelegate<bool> next, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
