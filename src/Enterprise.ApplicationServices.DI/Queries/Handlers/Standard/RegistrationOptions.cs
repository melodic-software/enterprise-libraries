using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Options;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

public sealed class RegistrationOptions<TQuery, TResponse> :
    RegistrationOptionsBase<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    /// <summary>
    /// Register decorators for the query handler.
    /// This is enabled by default.
    /// </summary>
    public bool UseDecorators { get; set; } = true;

    /// <summary>
    /// Provide custom factory delegates used for decorator composition.
    /// If none are provided, the default decorators will be used.
    /// <see cref="UseDecorators"/> must be true, otherwise decorator registrations will be skipped.
    /// </summary>
    public IEnumerable<Func<IServiceProvider, IHandleQuery<TQuery, TResponse>, IHandleQuery<TQuery, TResponse>>> DecoratorFactories { get; } = [];

    /// <summary>
    /// A factory method delegate that instantiates the query handler instance.
    /// </summary>
    internal Func<IServiceProvider, IHandleQuery<TQuery, TResponse>>? QueryHandlerFactory { get; }

    public RegistrationOptions(Func<IServiceProvider, IHandleQuery<TQuery, TResponse>>? factory)
    {
        QueryHandlerFactory = factory;
    }
}
