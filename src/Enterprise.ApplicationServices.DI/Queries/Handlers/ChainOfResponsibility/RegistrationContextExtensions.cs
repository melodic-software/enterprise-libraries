using Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;
using Enterprise.DI.Registration.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;

internal static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleQuery<TQuery, TResult>> AddChainOfResponsibility<TQuery, TResult>(
        this RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext,
        RegistrationOptions<TQuery, TResult> options,
        IServiceCollection services)
        where TQuery : class, IQuery
    {
        if (options.ConfigureChainOfResponsibility == null)
        {
            if (options.HandlerImplementationFactory == null)
            {
                throw new InvalidOperationException(
                    "A handler implementation factory must be configured for query handler registrations " +
                    "that use the chain of responsibility design pattern."
                );
            }

            services.RegisterDefaultChainOfResponsibility(options.HandlerImplementationFactory, options.ServiceLifetime);
        }
        else
        {
            // Initialize a builder instance that can be used to customize the chain.
            ResponsibilityChainRegistrationBuilder<TQuery, TResult> chainRegistrationBuilder =
                services.RegisterChainOfResponsibility<TQuery, TResult>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        return registrationContext;
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResult>> AddQueryHandler<TQuery, TResult>(
        this RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext,
        RegistrationOptions<TQuery, TResult> options)
        where TQuery : class, IQuery
    {
        // Register the standard.
        registrationContext.Add(
            ServiceDescriptor.Describe(
                typeof(IHandleQuery<TQuery, TResult>),
                ImplementationFactory<TQuery, TResult>,
                options.ServiceLifetime
            )
        );

        // Register the unbound.
        registrationContext.Add(
            ServiceDescriptor.Describe(
                typeof(IHandleQuery<TResult>),
                ImplementationFactory<TQuery, TResult>,
                options.ServiceLifetime
            )
        );

        // Register the non-generic.
        registrationContext.Add(
            ServiceDescriptor.Describe(
                typeof(IHandleQuery),
                ImplementationFactory<TQuery, TResult>,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }
    
    public static QueryHandler<TQuery, TResult> ImplementationFactory<TQuery, TResult>(IServiceProvider provider)
        where TQuery : class, IQuery
    {
        IResponsibilityChain<TQuery, TResult> responsibilityChain = provider.GetRequiredService<IResponsibilityChain<TQuery, TResult>>();

        // This is a query handler implementation that takes in a responsibility chain.
        return new QueryHandler<TQuery, TResult>(responsibilityChain);
    }
}
