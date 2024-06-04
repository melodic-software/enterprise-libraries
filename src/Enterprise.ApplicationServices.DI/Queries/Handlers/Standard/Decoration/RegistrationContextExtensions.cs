using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration.Delegates;
using Enterprise.DI.Core.Registration;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration;

public static class RegistrationContextExtensions
{
    public static void WithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        params QueryHandlerDecoratorImplementationFactory<TQuery, TResponse>[] decoratorFactories)
        where TQuery : IBaseQuery
    {
        registrationContext.WithDecorators(decoratorFactories.AsEnumerable());
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> WithDefaultDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext)
        where TQuery : IBaseQuery
    {
        IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResponse>>
            defaultDecoratorImplementationFactories = QueryHandlerDecoratorImplementationFactories.GetDefault<TQuery, TResponse>();

        registrationContext.WithDecorators(defaultDecoratorImplementationFactories);

        return registrationContext;
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> RegisterWithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        registrationContext.AddQueryHandler(options);

        if (options.DecoratorFactories.Any())
        {
            registrationContext.WithDecorators(options.DecoratorFactories);
        }
        else
        {
            registrationContext.WithDefaultDecorators();
        }

        return registrationContext;
    }

    private static void WithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResponse>> implementationFactories) 
        where TQuery : IBaseQuery
    {
        foreach (QueryHandlerDecoratorImplementationFactory<TQuery, TResponse> implementationFactory in implementationFactories)
        {
            registrationContext.WithDecorator(implementationFactory.Invoke);
        }
    }
}
