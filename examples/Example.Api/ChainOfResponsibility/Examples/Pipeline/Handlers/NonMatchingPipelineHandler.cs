using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Example.Api.ChainOfResponsibility.Examples.Pipeline.Handlers;

public class NonMatchingPipelineHandler : IHandler<string, bool>
{
    public Task<bool> HandleAsync(string request, SuccessorDelegate<bool> next, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
