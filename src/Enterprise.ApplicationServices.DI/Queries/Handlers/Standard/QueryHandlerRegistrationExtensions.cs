using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

public static class QueryHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="factory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>> factory,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new RegistrationOptions<TQuery, TResponse>(factory);
        configureOptions?.Invoke(options);
        RegistrationContext<IHandleQuery<TQuery, TResponse>> registration = services.RegisterQueryHandler(options);
        options.PostConfigure?.Invoke(services, registration);
    }
}
