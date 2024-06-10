﻿using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration.Delegates;
using Enterprise.DI.Core.Registration.Context;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration;

public static class RegistrationContextExtensions
{
    public static RegistrationContext<IHandleQuery<TQuery, TResult>> WithDecorators<TQuery, TResult>(
        this RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext,
        params QueryHandlerDecoratorImplementationFactory<TQuery, TResult>[] decoratorFactories)
        where TQuery : class, IQuery
    {
        return registrationContext.WithDecorators(decoratorFactories.AsEnumerable());
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResult>> WithDefaultDecorators<TQuery, TResult>(
        this RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext)
        where TQuery : class, IQuery
    {
        IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResult>>
            defaultDecoratorImplementationFactories = QueryHandlerDecoratorImplementationFactories.GetDefault<TQuery, TResult>();

        return registrationContext.WithDecorators(defaultDecoratorImplementationFactories);
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResult>> RegisterWithDecorators<TQuery, TResult>(
        this RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext,
        RegistrationOptions<TQuery, TResult> options)
        where TQuery : class, IQuery
    {
        registrationContext.AddQueryHandler(options);

        if (options.DecoratorFactories.Any())
        {
            return registrationContext.WithDecorators(options.DecoratorFactories);
        }

        return registrationContext.WithDefaultDecorators();
    }

    private static RegistrationContext<IHandleQuery<TQuery, TResult>> WithDecorators<TQuery, TResult>(
        this RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext,
        IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResult>> implementationFactories) 
        where TQuery : class, IQuery
    {
        foreach (QueryHandlerDecoratorImplementationFactory<TQuery, TResult> implementationFactory in implementationFactories)
        {
            registrationContext.WithDecorator(implementationFactory.Invoke);
        }

        return registrationContext;
    }
}
