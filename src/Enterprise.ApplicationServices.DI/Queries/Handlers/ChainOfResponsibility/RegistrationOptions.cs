using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Options;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;
public class RegistrationOptions<TQuery, TResult> : RegistrationOptionsBase<TQuery, TResult>
    where TQuery : IBaseQuery
{
    /// <summary>
    /// A factory method delegate that instantiates the chain of responsibility handler instance.
    /// </summary>
    internal HandlerImplementationFactory<TQuery, TResult>? HandlerImplementationFactory { get; }

    /// <summary>
    /// Provide a custom responsibility chain configuration.
    /// If not provided, the default chain will be used.
    /// </summary>
    public ConfigureChainOfResponsibility<TQuery, TResult>? ConfigureChainOfResponsibility { get; set; }

    public RegistrationOptions(HandlerImplementationFactory<TQuery, TResult>? handlerImplementationFactory)
    {
        HandlerImplementationFactory = handlerImplementationFactory;
    }
}
