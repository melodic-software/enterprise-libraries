using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Registration;

public static class RegistrationExtensions
{
    public static void RegisterChainOfResponsibility<TRequest>(this IServiceCollection services, params Type[] chainLinkTypes)
    {
        Type serviceType = typeof(IChainLink<TRequest>);
        Type[] typeArguments = [typeof(TRequest)];
        RegisterChainLinkTypes(services, serviceType, chainLinkTypes, typeArguments);

        // Register the responsibility chain itself.
        services.AddTransient<IResponsibilityChain<TRequest>>(provider =>
        {
            IEnumerable<IChainLink<TRequest>> handlers = provider.GetServices<IChainLink<TRequest>>();
            return new ResponsibilityChain<TRequest>(handlers);
        });
    }

    public static void RegisterChainOfResponsibility<TRequest, TResponse>(this IServiceCollection services, params Type[] chainLinkTypes)
    {
        Type serviceType = typeof(IChainLink<TRequest, TResponse>);
        Type[] typeArguments = [typeof(TRequest), typeof(TResponse)];
        RegisterChainLinkTypes(services, serviceType, chainLinkTypes, typeArguments);

        // Register the responsibility chain itself.
        services.AddTransient<IResponsibilityChain<TRequest, TResponse>>(provider =>
        {
            IEnumerable<IChainLink<TRequest, TResponse?>> handlers = provider.GetServices<IChainLink<TRequest, TResponse?>>();
            return new ResponsibilityChain<TRequest, TResponse>(handlers);
        });
    }

    private static void RegisterChainLinkTypes(IServiceCollection services, Type serviceType, Type[] chainLinkTypes, Type[] typeArguments)
    {
        foreach (Type type in chainLinkTypes)
        {
            Type typeToRegister;

            if (type.IsGenericTypeDefinition)
            {
                if (type.GetGenericArguments().Length != typeArguments.Length)
                {
                    throw new ArgumentException($"The number of generic type arguments provided does not match the number required for {type.Name}.");
                }
                typeToRegister = type.MakeGenericType(typeArguments);
            }
            else
            {
                typeToRegister = type;
            }

            if (!serviceType.IsAssignableFrom(typeToRegister))
            {
                throw new ArgumentException($"Type {typeToRegister.Name} does not implement {serviceType.Name}.");
            }

            services.AddTransient(serviceType, typeToRegister);
        }
    }
}
