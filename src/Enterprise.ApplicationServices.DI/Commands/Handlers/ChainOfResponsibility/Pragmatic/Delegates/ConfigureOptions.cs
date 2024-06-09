using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;

public delegate void ConfigureOptions<TCommand, TResult>(RegistrationOptions<TCommand, TResult> options)
    where TCommand : class, ICommand<TResult>;
