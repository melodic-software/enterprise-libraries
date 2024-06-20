﻿using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Shared.Delegates;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Bound;

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
        Delegates.QueryHandlerImplementationFactory<TQuery, TResult> implementationFactory,
        ConfigureOptions<TQuery, TResult>? configureOptions = null)
        where TQuery : class, IQuery<TResult>
    {
        services.Register((provider) => implementationFactory(provider), configureOptions);
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
        services.Register(
            QueryHandlerImplementationFactories.CreateSimpleQueryHandler<TQuery, TResult>,
            configureOptions
        );
    }
}
