﻿using Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;

internal static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> AddChainOfResponsibility<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        RegistrationOptions<TQuery, TResponse> options,
        IServiceCollection services)
        where TQuery : IBaseQuery
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
            ResponsibilityChainRegistrationBuilder<TQuery, TResponse> chainRegistrationBuilder =
                services.RegisterChainOfResponsibility<TQuery, TResponse>(options.ServiceLifetime);

            // Allow the caller to completely configure using the builder.
            options.ConfigureChainOfResponsibility(chainRegistrationBuilder);
        }

        return registrationContext;
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> AddQueryHandler<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        // Register the primary.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleQuery<TQuery, TResponse>),
                factory: ImplementationFactory<TQuery, TResponse>,
                options.ServiceLifetime
            )
        );

        // Register the alternative.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleQuery<TResponse>),
                factory: ImplementationFactory<TQuery, TResponse>,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }
    
    public static QueryHandler<TQuery, TResponse> ImplementationFactory<TQuery, TResponse>(IServiceProvider provider)
        where TQuery : IBaseQuery
    {
        IResponsibilityChain<TQuery, TResponse> responsibilityChain = provider.GetRequiredService<IResponsibilityChain<TQuery, TResponse>>();

        // This is a query handler implementation that takes in a responsibility chain.
        return new QueryHandler<TQuery, TResponse>(responsibilityChain);
    }
}