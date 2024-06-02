﻿using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration;

public static class RegistrationExtensions
{
    public static void WithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        params Func<IServiceProvider, IHandleQuery<TQuery, TResponse>, IHandleQuery<TQuery, TResponse>>[] decoratorFactories)
        where TQuery : IBaseQuery
    {
        registrationContext.WithDecorators(decoratorFactories);
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> WithDefaultDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext)
        where TQuery : IBaseQuery
    {
        IEnumerable<Func<IServiceProvider, IHandleQuery<TQuery, TResponse>, IHandleQuery<TQuery, TResponse>>>
            decoratorFactories = QueryHandlerDecoratorFactories.GetDefault<TQuery, TResponse>();

        return registrationContext.WithDecorators(decoratorFactories.ToArray());
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> RegisterWithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registration,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        registration.AddQueryHandler(options);

        if (options.DecoratorFactories.Any())
        {
            registration.WithDecorators(options.DecoratorFactories.ToArray());
        }
        else
        {
            registration.WithDefaultDecorators();
        }

        return registration;
    }
}