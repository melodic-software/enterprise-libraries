using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Shared.Delegates;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

public static class QueryHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResult>(this IServiceCollection services,
        QueryHandlerImplementationFactory<TQuery, TResult> implementationFactory,
        ConfigureOptions<TQuery, TResult>? configureOptions = null)
        where TQuery : class, IQuery
    {
        services.Register(implementationFactory, configureOptions);
    }

    /// <summary>
    /// Register a simple query handler.
    /// This expects that a separate registration of <see cref="IQueryLogic{TQuery,TResult}"/> has been made.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterSimpleQueryHandler<TQuery, TResult>(this IServiceCollection services,
        ConfigureOptions<TQuery, TResult>? configureOptions = null)
        where TQuery : class, IQuery
    {
        QueryHandlerImplementationFactory<TQuery, TResult> implementationFactory =
            QueryHandlerImplementationFactories.CreateSimpleQueryHandler<TQuery, TResult>;

        services.Register(implementationFactory, configureOptions);
    }

    private static void Register<TQuery, TResult>(this IServiceCollection services,
        QueryHandlerImplementationFactory<TQuery, TResult> implementationFactory,
        ConfigureOptions<TQuery, TResult>? configureOptions = null)
        where TQuery : class, IQuery
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        var options = new RegistrationOptions<TQuery, TResult>(implementationFactory);
        configureOptions?.Invoke(options);

        RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext =
            services.RegisterQueryHandler(options);

        options.PostConfigure?.Invoke(services, registrationContext);
    }

    private static RegistrationContext<IHandleQuery<TQuery, TResult>> RegisterQueryHandler<TQuery, TResult>(
        this IServiceCollection services,
        RegistrationOptions<TQuery, TResult> options)
        where TQuery : class, IQuery
    {
        RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext = services
            .BeginRegistration<IHandleQuery<TQuery, TResult>>();

        if (options.UseDecorators)
        {
            return registrationContext.RegisterWithDecorators(options);
        }

        return registrationContext.AddQueryHandler(options);
    }
}
