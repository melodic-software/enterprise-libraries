using Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers.Abstract;
using Enterprise.ApplicationServices.Core.Queries.Model;
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
        Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>> factory,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new RegistrationOptions<TQuery, TResponse>(factory);
        configureOptions?.Invoke(options);
        options.PostConfigure?.Invoke(services, services.RegisterQueryHandler(options));
    }
}
