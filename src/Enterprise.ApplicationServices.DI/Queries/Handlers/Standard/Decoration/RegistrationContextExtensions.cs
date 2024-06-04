using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration.Delegates;
using Enterprise.DI.Core.Registration;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration;

public static class RegistrationContextExtensions
{
    public static RegistrationContext<IHandleQuery<TQuery, TResponse>> WithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        params QueryHandlerDecoratorImplementationFactory<TQuery, TResponse>[] decoratorFactories)
        where TQuery : IBaseQuery
    {
        return registrationContext.WithDecorators(decoratorFactories.AsEnumerable());
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> WithDefaultDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext)
        where TQuery : IBaseQuery
    {
        IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResponse>>
            defaultDecoratorImplementationFactories = QueryHandlerDecoratorImplementationFactories.GetDefault<TQuery, TResponse>();

        return registrationContext.WithDecorators(defaultDecoratorImplementationFactories);
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> RegisterWithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        registrationContext.AddQueryHandler(options);

        if (options.DecoratorFactories.Any())
        {
            return registrationContext.WithDecorators(options.DecoratorFactories);
        }

        return registrationContext.WithDefaultDecorators();
    }

    private static RegistrationContext<IHandleQuery<TQuery, TResponse>> WithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResponse>> implementationFactories) 
        where TQuery : IBaseQuery
    {
        foreach (QueryHandlerDecoratorImplementationFactory<TQuery, TResponse> implementationFactory in implementationFactories)
        {
            registrationContext.WithDecorator(implementationFactory.Invoke);
        }

        return registrationContext;
    }
}
