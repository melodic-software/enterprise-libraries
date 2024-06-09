using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Pragmatic.Delegates;

public delegate void ConfigureOptions<TCommand, TResult>(RegistrationOptions<TCommand, TResult> options)
    where TCommand : class, ICommand<TResult>;
