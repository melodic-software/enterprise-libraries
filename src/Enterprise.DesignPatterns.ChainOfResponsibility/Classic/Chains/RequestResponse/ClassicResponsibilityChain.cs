using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestResponse;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestResponse;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains.RequestResponse;


public class ClassicResponsibilityChain<TRequest, TResponse> : IResponsibilityChain<TRequest, TResponse> where TRequest : class
{
    private readonly IReadOnlyCollection<IClassicHandler<TRequest, TResponse?>> _handlers;

    public ClassicResponsibilityChain(IEnumerable<IClassicHandler<TRequest, TResponse?>> handlers)
    {
        _handlers = handlers.ToList();
    }

    public TResponse? Handle(TRequest request)
    {
        TResponse? response = default;

        foreach (IClassicHandler<TRequest, TResponse?> handler in _handlers)
        {
            response = handler.Handle(request);
        }

        return response;
    }
}
