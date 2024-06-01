using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Options;

public sealed class QueryHandlerRegistrationOptions<TQuery, TResponse> where TQuery : IQuery
{
    /// <summary>
    /// A factory method delegate that instantiates the query handler instance.
    /// </summary>
    internal Func<IServiceProvider, IHandleQuery<TQuery, TResponse>>? QueryHandlerFactory { get; }

    /// <summary>
    /// A factory method delegate that instantiates the chain of responsibility instance.
    /// </summary>
    internal Func<IServiceProvider, IHandler<TQuery, TResponse>>? ChainOfResponsibilityHandlerFactory { get; }

    /// <summary>
    /// Use the default query handler decorators?
    /// </summary>
    internal bool UseDecorators { get; set; }

    /// <summary>
    /// Use the default query handling responsibility chain?
    /// </summary>
    internal bool UseChainOfResponsibility { get; set; }

    /// <summary>
    /// This sets the service lifetime for the query handler registration.
    /// The default is a transient service lifetime, and is recommended for most registrations.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Provide a custom responsibility chain configuration.
    /// </summary>
    public Action<ResponsibilityChainRegistrationBuilder<TQuery, TResponse>>? ConfigureChainOfResponsibility { get; set; }

    /// <summary>
    /// Provider further configuration to the query handler registration.
    /// </summary>
    public Action<IServiceCollection, RegistrationContext<IHandleQuery<TQuery, TResponse>>>? PostConfigure { get; set; }

    public QueryHandlerRegistrationOptions(Func<IServiceProvider, IHandleQuery<TQuery, TResponse>>? factory)
    {
        QueryHandlerFactory = factory;
        UseDecorators = true;
    }

    public QueryHandlerRegistrationOptions(Func<IServiceProvider, IHandler<TQuery, TResponse>>? factory)
    {
        ChainOfResponsibilityHandlerFactory = factory;
        UseChainOfResponsibility = true;
    }
}
