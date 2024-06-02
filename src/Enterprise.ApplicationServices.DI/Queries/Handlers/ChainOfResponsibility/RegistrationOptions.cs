﻿using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Options;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;
public class RegistrationOptions<TQuery, TResponse> : RegistrationOptionsBase<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    /// <summary>
    /// A factory method delegate that instantiates the chain of responsibility instance.
    /// </summary>
    internal Func<IServiceProvider, IHandler<TQuery, TResponse>>? QueryHandlerFactory { get; }

    /// <summary>
    /// Provide a custom responsibility chain configuration.
    /// If not provided, the default chain will be used.
    /// </summary>
    public Action<ResponsibilityChainRegistrationBuilder<TQuery, TResponse>>? ConfigureChainOfResponsibility { get; set; }

    public RegistrationOptions(Func<IServiceProvider, IHandler<TQuery, TResponse>>? factory)
    {
        QueryHandlerFactory = factory;
    }
}