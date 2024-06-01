using Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Options;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.ChainOfResponsibility;

public static class ChainOfResponsibilityRegistrationExtensions
{
    public static void RegisterDefaultChainOfResponsibility<TQuery, TResponse>(
        this IServiceCollection services,
        Func<IServiceProvider, IHandler<TQuery, TResponse>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TQuery : IQuery
    {
        services.RegisterChainOfResponsibility<TQuery, TResponse>()
            .WithSuccessor<LoggingQueryHandler<TQuery, TResponse>>()
            .WithSuccessor<ErrorHandlingQueryHandler<TQuery, TResponse>>()
            .WithSuccessor<NullQueryValidationQueryHandler<TQuery, TResponse>>()
            .WithSuccessor<FluentValidationQueryHandler<TQuery, TResponse>>()
            .WithSuccessor(factory, serviceLifetime);
    }

    internal static void AddChainOfResponsibility<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registration,
        QueryHandlerRegistrationOptions<TQuery, TResponse> options,
        IServiceCollection services)
        where TQuery : IQuery
    {
        if (options.ConfigureChainOfResponsibility == null)
        {
            if (options.ChainOfResponsibilityHandlerFactory == null)
            {
                throw new InvalidOperationException(
                    "A handler factory must be configured for query handler registrations " +
                    "that use the chain of responsibility design pattern."
                );
            }

            services.RegisterDefaultChainOfResponsibility(
                options.ChainOfResponsibilityHandlerFactory,
                options.ServiceLifetime
            );
        }
        else
        {
            // Initialize a builder instance that can be used to customize the chain.
            ResponsibilityChainRegistrationBuilder<TQuery, TResponse> chainRegistrationBuilder = services
                .RegisterChainOfResponsibility<TQuery, TResponse>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        // This is a query handler implementation that takes in a responsibility chain.
        registration.Add(provider =>
        {
            IResponsibilityChain<TQuery, TResponse> responsibilityChain = provider
                .GetRequiredService<IResponsibilityChain<TQuery, TResponse>>();

            return new QueryHandler<TQuery, TResponse>(responsibilityChain);

        }, options.ServiceLifetime);
    }
}
