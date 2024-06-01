using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;

public static class QueryHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler using the chain of responsibility design pattern.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="factory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, ApplicationServices.ChainOfResponsibility.Queries.Handlers.Abstract.QueryHandlerBase<TQuery, TResponse>> factory,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new RegistrationOptions<TQuery, TResponse>(factory);
        configureOptions?.Invoke(options);
        RegistrationContext<IHandleQuery<TQuery, TResponse>> registration = services.RegisterQueryHandler(options);
        options.PostConfigure?.Invoke(services, registration);
    }

    private static RegistrationContext<IHandleQuery<TQuery, TResponse>> RegisterQueryHandler<TQuery, TResponse>(
        this IServiceCollection services,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        return services
            .BeginRegistration<IHandleQuery<TQuery, TResponse>>()
            .AddChainOfResponsibility(options, services);
    }
}
