﻿using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Alternate;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate;

public class RegistrationOptions<TCommand, TResponse> : RegistrationOptionsBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// A factory method delegate that instantiates the chain of responsibility instance.
    /// </summary>
    internal Func<IServiceProvider, IHandler<TCommand, TResponse>>? CommandHandlerFactory { get; }

    /// <summary>
    /// Provide a custom responsibility chain configuration.
    /// If not provided, the default chain will be used.
    /// </summary>
    public Action<ResponsibilityChainRegistrationBuilder<TCommand, TResponse>>? ConfigureChainOfResponsibility { get; set; }

    public RegistrationOptions(Func<IServiceProvider, IHandler<TCommand, TResponse>>? factory)
    {
        CommandHandlerFactory = factory;
    }
}