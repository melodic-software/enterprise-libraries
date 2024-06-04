using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Options;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;
public class RegistrationOptions<TQuery, TResponse> : RegistrationOptionsBase<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    /// <summary>
    /// A factory method delegate that instantiates the chain of responsibility handler instance.
    /// </summary>
    internal HandlerImplementationFactory<TQuery, TResponse>? HandlerImplementationFactory { get; }

    /// <summary>
    /// Provide a custom responsibility chain configuration.
    /// If not provided, the default chain will be used.
    /// </summary>
    public ConfigureChainOfResponsibility<TQuery, TResponse>? ConfigureChainOfResponsibility { get; set; }

    public RegistrationOptions(HandlerImplementationFactory<TQuery, TResponse>? handlerImplementationFactory)
    {
        HandlerImplementationFactory = handlerImplementationFactory;
    }
}
