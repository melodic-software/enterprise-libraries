using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Simple;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

public static class CommandHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>> implementationFactory,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        services.Register(implementationFactory, configureOptions);
    }

    /// <summary>
    /// Register a simple query handler.
    /// This expects that a separate registration of <see cref="IQueryLogic{TQuery,TResponse}"/> has been made.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterSimpleQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>> implementationFactory = 
            SimpleQueryHandlerFactoryService.GetImplementationFactory<TQuery, TResponse>();

        services.Register(implementationFactory, configureOptions);
    }

    private static void Register<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>> implementationFactory,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        var options = new RegistrationOptions<TQuery, TResponse>(implementationFactory);
        configureOptions?.Invoke(options);

        RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext =
            services.RegisterQueryHandler(options);

        options.PostConfigure?.Invoke(services, registrationContext);
    }

    private static RegistrationContext<IHandleQuery<TQuery, TResponse>> RegisterQueryHandler<TQuery, TResponse>(
        this IServiceCollection services,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext = services
            .BeginRegistration<IHandleQuery<TQuery, TResponse>>();

        if (options.UseDecorators)
        {
            registrationContext.RegisterWithDecorators(options);
        }
        else
        {
            registrationContext.AddQueryHandler(options);
        }

        return registrationContext;
    }
}
