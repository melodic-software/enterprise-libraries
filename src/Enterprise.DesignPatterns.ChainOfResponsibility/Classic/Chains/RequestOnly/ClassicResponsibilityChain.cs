using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestOnly;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestOnly;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestOnly;

public class ClassicResponsibilityChain<TRequest> : IResponsibilityChain<TRequest> where TRequest : class
{
    private readonly IReadOnlyCollection<IClassicHandler<TRequest>> _handlers;

    public ClassicResponsibilityChain(IEnumerable<IClassicHandler<TRequest>> handlers)
    {
        _handlers = handlers.ToList();
    }

    public void Handle(TRequest request)
    {
        foreach (IClassicHandler<TRequest> handler in _handlers)
        {
            handler.Handle(request);
        }
    }
}
