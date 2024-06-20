using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Options;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration.Delegates;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

public sealed class RegistrationOptions<TQuery, TResult> :
    RegistrationOptionsBase<TQuery, TResult>
    where TQuery : class, IQuery
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
    public IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResult>> DecoratorFactories { get; } = [];

    /// <summary>
    /// A factory method delegate that instantiates the query handler instance.
    /// </summary>
    internal Func<IServiceProvider, IHandleQuery<TQuery, TResult>>? QueryHandlerImplementationFactory { get; }

    public RegistrationOptions(Func<IServiceProvider, IHandleQuery<TQuery, TResult>>? queryHandlerImplementationFactory)
    {
        QueryHandlerImplementationFactory = queryHandlerImplementationFactory;
    }
}
